using ExtendedNumerics.Reflection;
using HbaseReportService.Commons.Helpers;
using HbaseReportService.Hbase;
using HbaseReportService.Hbase.Filters;
using HbaseReportService.Hbase.Generated;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HbaseReportService.Services.Base
{
    public  class ReportBaseService : IReportBaseService
    {

        protected readonly IHBaseClient _hbaseClient;
        protected readonly ILogger<ReportBaseService> _logger;

        public ReportBaseService(ILogger<ReportBaseService> logger, IHBaseClient hbaseClient)
        {
            _hbaseClient = hbaseClient;
            _logger = logger;
        }

        public async Task<IEnumerable<T>> GetDataHbaseWithPage<T>(int companyId, long[] vehicleIds, DateTime fromDate, DateTime toDate, int pageIndex, int pageSize) where T : IHbaseTable, new()
        {
            IEnumerable<T> data = null;

            try
            {
                var table = "Report.Stops";
                int firstPage = (pageIndex - 1) * pageSize;
                int endPage = firstPage + pageSize;
                var scanSetting = new Scanner();
                scanSetting.caching = 1000;
                var filterMultiRange = new MultiRowRangeFilter();
                var filterListRange = new FilterList(FilterList.Operator.MustPassAll, new KeyOnlyFilter(), new FirstKeyOnlyFilter());
                foreach (var vehicleId in vehicleIds)
                {
                    var startRowV = $"{HBaseHelpers.HbaseSystemKey}{companyId}{vehicleId}{HBaseHelpers.SplitSchemaCharacter}{((DateTimeOffset)fromDate).ToUnixTimeMilliseconds()}";
                    var endRowV = $"{HBaseHelpers.HbaseSystemKey}{companyId}{vehicleId}{HBaseHelpers.SplitSchemaCharacter}{((DateTimeOffset)toDate).ToUnixTimeMilliseconds()}";
                    var ranges = new RowRange(startRowV, true, endRowV, false);
                    filterMultiRange.rangeList.Add(ranges);
                }
                filterListRange.AddFilter(filterMultiRange);
                // TODO check lại có nhất thiết cần FirstKey
                scanSetting.filter = filterListRange.ToEncodedString();

                var scannerInfo = await _hbaseClient.CreateScanner(table, scanSetting);
                CellSet next;
                List<CellSet.Row> rows = new List<CellSet.Row>();
                while ((next = await _hbaseClient.ScannerGetNext<CellSet>(scannerInfo)) != null)
                {
                    rows = rows.Concat(next.rows).ToList();
                }
                if (scannerInfo != null)
                {
                    await _hbaseClient.DeleteScannerAsync(table, scannerInfo);
                }
                if (rows.Count > 0)
                {
                    var endRow = endPage > rows.Count() ? rows.Count() : endPage;
                    var cellData = await _hbaseClient.GetCellsAsync<CellSet>(table, rows.GetRange(firstPage, endRow).Select(x => HttpUtility.UrlEncode(Encoding.UTF8.GetString(x.key))).ToArray());
                    if (cellData != null)
                    {
                        data = ToReals<T>(cellData.rows);
                    }
                }
            }
            catch (Exception ex)
            {

                 _logger.LogError($"Lỗi {MethodHelper.GetNameAsync()}: {ex.Message}");
            }

            return data;
        }

        public Dictionary<string, HbaseColumnAttribute> GetPropertyNameWithFamilyStr<T>()
        {
            var hcaType = typeof(HbaseColumnAttribute);
            var properties = typeof(T).GetProperties()
                .ToList();

            var result = new Dictionary<string, HbaseColumnAttribute>();

            foreach (var property in properties)
            {
                var att = property.GetCustomAttribute(hcaType, true) as HbaseColumnAttribute;
                if (att == null) continue;
                if (string.IsNullOrEmpty(att.Family))
                {
                    att.Family = HbaseColumnAttribute.DefaultFamily;
                }
                if (string.IsNullOrEmpty(att.Column))
                {
                    att.Column = property.Name;
                }
                result.Add(property.Name, att);
            }

            return result;
        }

        public List<T> ToReals<T>(IEnumerable<CellSet.Row> trrs) where T : IHbaseTable, new()
        {
            var nameWithFamily = GetPropertyNameWithFamilyStr<T>();
            var properties = typeof(T).GetProperties()
                .Where(t => nameWithFamily.ContainsKey(t.Name))
                .ToList();

            var result = new List<T>();
            foreach (var trr in trrs)
            {
                var real = new T
                {
                    RowKey = trr.key.ToObject<string>()
                };
                try
                {
                    var dict = trr.values.ToDictionary(t => t.column.ToObject<string>());
                    foreach (var property in properties)
                    {
                        try
                        {
                            if (dict.TryGetValue($"{nameWithFamily[property.Name].Family}:{nameWithFamily[property.Name].Column}", out var tCell))
                            {
                                object v = tCell.data.ToObject(property.PropertyType);
                                property.SetValue(real, v);
                            }
                        }
                        catch (Exception e)
                        {
                             _logger.LogError($"Lỗi {MethodHelper.GetNameAsync()}: {e.Message}");
                        }

                    }
                }
                catch (Exception ex)
                {

                    _logger.LogError($"Lỗi {MethodHelper.GetNameAsync()}: {ex.Message}");
                }

                result.Add(real);
            }

            return result;
        }
    }
}
