using HbaseReportService.Hbase;
using NPOI.SS.Formula.Functions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace HbaseReportService.Entities
{
    public class ReportBase
    {
        [Key]
        [Column("FK_VehicleID")]
        [HbaseColumn(Column = "2", IsSum = true)]
        public virtual long VehicleId { get; set; }

        [Column("FK_CompanyID")]
        [HbaseColumn(Column = "1")]
        public virtual int CompanyId { get; set; }

        [Column("StartTime")]
        [HbaseColumn(Column = "5")]
        public virtual DateTime StartTime { get; set; }

        [Column("EndTime")]
        [HbaseColumn(Column = "6")]
        public virtual DateTime? EndTime { get; set; }
    }
}
