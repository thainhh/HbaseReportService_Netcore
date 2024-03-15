using System;

namespace HbaseReportService.Hbase
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HbaseTableAttribute : Attribute
    {
        public string Table { get; set; }
        public HbaseTableAttribute(string table = null)
        {
            Table = table;
        }
    }
}
