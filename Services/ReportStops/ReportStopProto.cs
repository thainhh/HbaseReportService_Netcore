using Grpc.Core;
using HbaseReportService.Entities;
using HbaseReportService.Services.Base;
using System;
using System.Threading.Tasks;

namespace HbaseReportService.Services.ReportStops
{
    public class ReportStopProto:Stop.StopBase
    {
        private readonly IReportBaseService _reportBaseService;

        public ReportStopProto(IReportBaseService reportBaseService)
        {
            _reportBaseService = reportBaseService;
        }


        public override Task<ReportStopReply> GetReportStopPage(ReportStopRequest request, ServerCallContext context)
        {
            long[] vehicleIds = new long[] { 485127 };
            var data = _reportBaseService.GetDataHbaseWithPage<StopHbase>(38040, vehicleIds, new DateTime(2023, 04, 01, 00, 00, 00), new DateTime(2024, 04, 01, 00, 00, 00), 1, 10);

            return base.GetReportStopPage(request, context);
        }
    }
}
