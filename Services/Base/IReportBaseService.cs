using HbaseReportService.Hbase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HbaseReportService.Services.Base
{
    public interface IReportBaseService
    {
        Task<IEnumerable<T>> GetDataHbaseWithPage<T>(int companyId, long[] vehicleIds, DateTime fromDate, DateTime toDate, int pageIndex, int pageSize) where T : IHbaseTable, new();

    }
}
