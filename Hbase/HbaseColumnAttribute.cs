using System;

namespace HbaseReportService.Hbase
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HbaseColumnAttribute : Attribute
    {
        public const string DefaultFamily = "d";
        public string Family { get; set; }
        public string Column { get; set; }
        public bool IsSum { get; set; }
        public HbaseColumnAttribute(string family = DefaultFamily, string column = null, bool sum = false)
        {
            Family = family;
            Column = column;
            IsSum = sum;
        }
    }
}
