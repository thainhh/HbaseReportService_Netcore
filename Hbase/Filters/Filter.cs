using HbaseReportService.Hbase.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HbaseReportService.Hbase.Filters
{
    public abstract class Filter
    {
        /// <summary>
        /// Generates an encoded string that can be applied to a <see cref="Scanner.filter"/>.
        /// </summary>
        /// <returns>
        /// A filter string.
        /// </returns>
        public abstract string ToEncodedString();
    }
}
