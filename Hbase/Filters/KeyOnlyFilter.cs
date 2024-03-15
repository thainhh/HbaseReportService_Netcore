using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HbaseReportService.Hbase.Filters
{
    /// <summary>
    /// A filter that will only return the key component of each KV (the value will be rewritten as empty).
    /// </summary>
    /// <remarks>
    /// This filter can be used to grab all of the keys without having to also grab the values.
    /// </remarks>
    public class KeyOnlyFilter : Filter
    {
        // note:  could not get "lenAsVal" to stringify, so it has been removed.

        /// <inheritdoc/>
        public override string ToEncodedString()
        {
            return @"{""type"":""KeyOnlyFilter""}";
        }
    }
}
