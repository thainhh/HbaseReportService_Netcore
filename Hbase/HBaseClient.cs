using HbaseReportService.Hbase.Generated;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Ocsp;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HbaseReportService.Hbase
{
    public class HBaseClient : IHBaseClient
    {
        private readonly IOptions<HbaseOption> options;
        private readonly CredentialCache credentialCache;
        private readonly string contentType = "application/x-protobuf";
        public HBaseClient(IOptions<HbaseOption> _options)
        {
            this.options = _options;
            credentialCache = new CredentialCache();
            credentialCache.Add(new Uri(options.Value.IPAddresses), "Basic", new NetworkCredential(this.options.Value.UserName, this.options.Value.Password.ToSecureString()));
        }
        private async Task<HttpWebResponse> IssueWebRequest(
            string endpoint, string query, string method, Stream input)
        {
            Stopwatch watch = Stopwatch.StartNew();
            var url = new Uri(this.options.Value.IPAddresses);
            UriBuilder builder = new UriBuilder(
                url.Scheme,
                url.Host,
                options.Value.Port,
                options.Value.AlternativeEndpoint + endpoint);

            if (query != null)
            {
                builder.Query = query;
            }
            HttpWebRequest httpWebRequest = WebRequest.CreateHttp(builder.Uri);
            httpWebRequest.ServicePoint.ReceiveBufferSize = options.Value.ReceiveBufferSize;
            httpWebRequest.ServicePoint.UseNagleAlgorithm = options.Value.UseNagle;
            httpWebRequest.Timeout = options.Value.TimeoutMillis; // This has no influence for calls that are made Async
            httpWebRequest.KeepAlive = options.Value.KeepAlive;
            httpWebRequest.Credentials = credentialCache;
            httpWebRequest.PreAuthenticate = true;
            httpWebRequest.Method = method;
            httpWebRequest.Accept = httpWebRequest.ContentType = contentType;
            httpWebRequest.AllowAutoRedirect = false;
            if (input != null)
            {
                // expecting the caller to seek to the beginning or to the location where it needs to be copied from
                Stream req = null;
                try
                {
                    req = await httpWebRequest.GetRequestStreamAsync();

                    input.CopyTo(req);
                }
                catch (TimeoutException)
                {
                    httpWebRequest.Abort();
                    throw;
                }
                finally
                {
                    if (req != null)
                    {
                        req.Close();
                    }
                }
            }

            try
            {
                return (HttpWebResponse)await httpWebRequest.GetResponseAsync();
            }
            catch (Exception ex)
            {
                httpWebRequest.Abort();
                throw;
            }
        }
        /// <summary>
        /// Scan sẽ luôn quét tất cả các hàng (hoặc tất cả các hàng giữa hàng bắt đầu và hàng dừng mà bạn đã chỉ định) dù có bộ lọc thì nó vẫn...quét qua
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scannerInfo"></param>
        /// <returns></returns>
        public async Task<T> ScannerGetNext<T>(ScannerInformation scannerInfo)
        {
            using (var webResponse = await GetRequest(scannerInfo.TableName + "/scanner/" + scannerInfo.ScannerId, null))
            {
                var stream = webResponse.GetResponseStream();
                if (webResponse.StatusCode == HttpStatusCode.OK)
                {
                    return Serializer.Deserialize<T>(webResponse.GetResponseStream());
                }
                return default;
            }
        }
        public async Task<HttpWebResponse> PostRequest<TReq>(string endpoint, TReq request)
           where TReq : class
        {
            return await ExecuteMethod("POST", null, endpoint, request);
        }

        private async Task<HttpWebResponse> GetRequest(string endpoint, string query)
        {
            return await IssueWebRequest(endpoint, query, "GET", null);
        }
        private async Task<HttpWebResponse> DeleteRequest<TReq>(string endpoint, TReq request)
           where TReq : class
        {
            return await ExecuteMethod("DELETE", null, endpoint, request);
        }

        public async Task DeleteScannerAsync(string tableName, ScannerInformation scannerInfo)
        {
            using (HttpWebResponse webResponse = await DeleteRequest<Scanner>(tableName + "/scanner/" + scannerInfo.ScannerId, null))
            {
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    using (var output = new StreamReader(webResponse.GetResponseStream()))
                    {
                        string message = output.ReadToEnd();
                        throw new WebException(
                           string.Format(
                              "Couldn't delete scanner {0} associated with {1} table.! Response code was: {2}, expected 200! Response body was: {3}",
                              scannerInfo.ScannerId,
                              tableName,
                              webResponse.StatusCode,
                              message));
                    }
                }
            }
        }
        public async Task<ScannerInformation> CreateScanner(string tableName, Scanner scannerSettings)
        {
            using (HttpWebResponse response = await PostRequest(tableName + "/scanner", scannerSettings))
            {
                if (response.StatusCode != HttpStatusCode.Created)
                {
                    using (var output = new StreamReader(response.GetResponseStream()))
                    {
                        string message = output.ReadToEnd();
                        throw new WebException(
                            string.Format(
                                "Couldn't create a scanner for table {0}! Response code was: {1}, expected 201! Response body was: {2}",
                                tableName,
                                response.StatusCode,
                                message));
                    }
                }
                string location = response.Headers.Get("Location");
                if (location == null)
                {
                    throw new ArgumentException("Couldn't find header 'Location' in the response!");
                }

                return new ScannerInformation(new Uri(location), tableName, response.Headers);
            }
        }
        private async Task<HttpWebResponse> ExecuteMethod<TReq>(
           string method,
           string query,
           string endpoint,
           TReq request) where TReq : class
        {
            using (var input = new MemoryStream(this.options.Value.SerializationBufferSize))
            {
                if (request != null)
                {
                    Serializer.Serialize(input, request);
                }
                input.Seek(0, SeekOrigin.Begin);
                return await IssueWebRequest(endpoint, query, method, input);
            }
        }

        private async Task<T> GetRequestAndDeserializeAsync<T>(string endpoint, string query)
        {
            using (HttpWebResponse response = await IssueWebRequest(endpoint, query, "GET", null))
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    return Serializer.Deserialize<T>(responseStream);
                }
            }
        }
        /// <summary>
        /// MultiGet thực hiện tìm kiếm (theo một nghĩa nào đó) cho mỗi Get.
        /// Nếu số lượng Gets trong MultiGet rất nhỏ so với tổng số hàng, tốt hơn là sử dụng MultiGet. 
        /// Tuy nhiên, nếu bạn có thể chỉ định hàng bắt đầu và dừng trong thao tác Quét, quá trình quét sẽ nhanh hơn (vì bạn giới hạn số lượng hàng sẽ được đọc)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="rowKeys"></param>
        /// <returns></returns>
        public async Task<T> GetCellsAsync<T>(string tableName, string[] rowKeys)
        {
            string endpoint = tableName + "/multiget";

            string query = null;
            for (var i = 0; i < rowKeys.Length; i++)
            {
                var prefix = "&";
                if (i == 0)
                {
                    prefix = "";
                }

                query += prefix + "row=" + rowKeys[i];
            }

            return await GetRequestAndDeserializeAsync<T>(endpoint, query);
        }
        public async Task<T> GetSum<T>(string tableName, int companyId, long[] VehicleIds, DateTime fromDate, DateTime toDate, string[] cols)
        {
            var url = new Uri(this.options.Value.IPRestReport);
            HttpWebRequest httpWebRequest = WebRequest.CreateHttp(url);
            httpWebRequest.ServicePoint.ReceiveBufferSize = options.Value.ReceiveBufferSize;
            httpWebRequest.ServicePoint.UseNagleAlgorithm = options.Value.UseNagle;
            httpWebRequest.Timeout = options.Value.TimeoutMillis; // This has no influence for calls that are made Async
            httpWebRequest.KeepAlive = options.Value.KeepAlive;
            httpWebRequest.Credentials = credentialCache;
            httpWebRequest.PreAuthenticate = true;
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = httpWebRequest.ContentType = "application/json";
            httpWebRequest.AllowAutoRedirect = false;
            Stream req = null;
            try
            {
                req = await httpWebRequest.GetRequestStreamAsync();
                var requestData = new Dictionary<string, object>
            {
                { "companyid", companyId.ToString()},
                { "vehicleids", VehicleIds },
                { "fromdate", fromDate.ToString("dd-MM-yyyy")},
                { "todate", toDate.Date.AddDays(1).ToString("dd-MM-yyyy")},
                { "tablename", tableName},
                { "cols", cols}
            };
                using (var stream = new StreamWriter(req))
                {
                    stream.Write(System.Text.Json.JsonSerializer.Serialize(requestData));
                }

                using (var webResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync())
                {
                    if (webResponse.StatusCode == HttpStatusCode.OK)
                    {
                        using (var output = new StreamReader(webResponse.GetResponseStream()))
                        {
                            var content = output.ReadToEnd();
                            return System.Text.Json.JsonSerializer.Deserialize<T>(content);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                httpWebRequest.Abort();
                throw;
            }
            finally
            {
                if (req != null)
                {
                    req.Close();
                }
            }
            return default(T);
        }
    }
}
