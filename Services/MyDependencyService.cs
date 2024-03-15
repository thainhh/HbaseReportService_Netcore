using HbaseReportService.Hbase;
using HbaseReportService.Services.Base;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HbaseReportService.Services
{
    public class MyDependencyService : IMyDependencyService
    {
        protected readonly IHBaseClient _hbaseClient;
        protected readonly ILogger<MyDependencyService> _logger;

        public MyDependencyService(ILogger<MyDependencyService> logger, IHBaseClient hbaseClient)
        {
            _hbaseClient = hbaseClient;
            _logger = logger;
        }

        public string GetName(string _name)
        {
            return _name;
        }

        public Task<IEnumerable<T>> GetName<T>() where T : IHbaseTable, new()
        {
            throw new System.NotImplementedException();
        }
    }
}
