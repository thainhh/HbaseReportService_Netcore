using System.Collections;
using System.Threading.Tasks;

namespace HbaseReportService.Services.ReportStops
{
    public interface IReportStopService
    {
        Task<ReportStopReply> GetReportStop();
    }
}
