using HbaseReportService.Hbase.Generated;
using ProtoBuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HbaseReportService.Hbase
{
    public interface IHbaseParser
    {
        T ToReal<T>(CellSet.Row trr) where T : class, IHbaseTable, new();
        List<T> ToReals<T>(IEnumerable<CellSet.Row> trrs) where T : class, IHbaseTable, new();
    }
}
