using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HbaseReportService.Hbase.Filters
{
    public class QualifierFilter : CompareFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QualifierFilter"/> class.
        /// </summary>
        /// <param name="op">The op.</param>
        /// <param name="qualifierComparator">The qualifier comparator.</param>
        public QualifierFilter(CompareOp op, ByteArrayComparable qualifierComparator) : base(op, qualifierComparator)
        {
        }

        /// <inheritdoc/>
        public override string ToEncodedString()
        {
            const string filterPattern = @"{{""type"":""QualifierFilter"",""op"":""{0}"",""comparator"":{{{1}}}}}";
            return string.Format(CultureInfo.InvariantCulture, filterPattern, CompareOperation.ToCodeName(), Comparator.ToEncodedString());
        }
    }
}
