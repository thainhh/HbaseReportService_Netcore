using System;

namespace HbaseReportService.Hbase
{
    /// <summary>
    /// Instructs Code Analysis to treat a method as a validation method for a given parameter and not fire CA1062 when it is used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}
