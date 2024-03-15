using HbaseReportService.Hbase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HbaseReportService.Services
{
    public interface IMyDependencyService
    {
        string GetName(string _name);
        Task<IEnumerable<T>> GetName<T>() where T : IHbaseTable, new();
    }
}
