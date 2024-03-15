using HbaseReportService.Hbase.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HbaseReportService.Hbase
{
    public interface IHBaseClient
    {
        Task<ScannerInformation> CreateScanner(string tableName, Scanner scannerSettings);
        Task<T> ScannerGetNext<T>(ScannerInformation scannerInfo);
        Task<HttpWebResponse> PostRequest<TReq>(string endpoint, TReq request) where TReq : class;        
        Task<T> GetCellsAsync<T>(string tableName, string[] rowKeys);
        Task DeleteScannerAsync(string tableName, ScannerInformation scannerInfo);
        Task<T> GetSum<T>(string tableName, int companyId, long[] VehicleIds, DateTime fromDate, DateTime toDate, string[] cols);
    }
}
