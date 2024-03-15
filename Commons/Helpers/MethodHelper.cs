using System.Runtime.CompilerServices;

namespace HbaseReportService.Commons.Helpers
{
    public static class MethodHelper
    {
        public static string GetNameAsync([CallerMemberName] string name = "") => name;

    }
}
