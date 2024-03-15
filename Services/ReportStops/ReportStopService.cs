using HbaseReportService.Entities;
using HbaseReportService.Services.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace HbaseReportService.Services.ReportStops
{
    public class ReportStopService : IReportStopService
    {
        protected readonly ILogger<ReportStopService> _logger;
        protected readonly IReportBaseService _reportBaseService;

        public ReportStopService(ILogger<ReportStopService> logger, IReportBaseService reportBaseService)
        {
            _logger = logger;
            _reportBaseService = reportBaseService;
        }

        public async Task<ReportStopReply> GetReportStop()
        {
            ReportStopReply reply = null;

            long[] vehicleIds = new long[] { 485127 };
            var data = await _reportBaseService.GetDataHbaseWithPage<StopHbase>(38040, vehicleIds, new DateTime(2023, 04, 01, 00, 00, 00), new DateTime(2024, 04, 01, 00, 00, 00), 1, 10);



            return await Task.FromResult(reply);
        }
    }
}
