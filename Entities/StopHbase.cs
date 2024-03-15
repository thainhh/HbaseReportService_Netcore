using HbaseReportService.Hbase;
using System.ComponentModel.DataAnnotations.Schema;

namespace HbaseReportService.Entities
{
    [Table("Report.Stops")]
    [HbaseTable("Report.Stops")]
    public class StopHbase : ReportBase, IHbaseTable
    {
        [HbaseColumn(Column = "9", IsSum = true)]
        public int? TotalTimeStop { get; set; }

        [Column("Longitude")]
        [HbaseColumn(Column = "8")]
        public double StartLongitude { get; set; }

        [Column("Latitude")]
        [HbaseColumn(Column = "7")]
        public double StartLatitude { get; set; }

        [HbaseColumn(Column = "10", IsSum = true)]
        public int? MinutesOfManchineOn { get; set; }

        [HbaseColumn(Column = "13", IsSum = true)]
        public int? MinutesOfAirConditioningOn { get; set; }

        public bool IsValid { get; set; }

        [HbaseColumn(Column = "11")]
        public string DriverName { get; set; }

        [HbaseColumn(Column = "12")]
        public string DriverLicense { get; set; }

        [HbaseColumn(Column = "14")]
        public int? Vbefore { get; set; }

        [HbaseColumn(Column = "15")]
        public double? KmGPS { get; set; }

        [HbaseColumn(Column = "16")]
        public string Temperature { get; set; }

        public string RowKey { get; set; }
        
    }
}
