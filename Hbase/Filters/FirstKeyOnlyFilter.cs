using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HbaseReportService.Hbase.Filters
{
    /// <summary>
    /// A filter that will only return the first KV from each row.
    /// </summary>
    public class FirstKeyOnlyFilter : Filter
    {
        /// <inheritdoc/>
        public override string ToEncodedString()
        {
            return @"{""type"":""FirstKeyOnlyFilter""}";
        }
    }
}
