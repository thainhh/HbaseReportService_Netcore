using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HbaseReportService.Hbase
{
    public interface IHbaseTable
    {
        string RowKey { get; set; }
    }
}
