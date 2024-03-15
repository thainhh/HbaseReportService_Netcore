using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HbaseReportService.Hbase.Filters
{
    /// <summary>
    /// Class có thể gửi 2 constructor theo list byte
    /// </summary>
    public class MultiRowRangeFilter : Filter
    {
        public List<RowRange> rangeList;

        /**
         * @param list A list of <code>RowRange</code>
         */
        public MultiRowRangeFilter()
        {
            rangeList = new List<RowRange>();
        }
        /// <summary>
        /// list - A list of RowRange
        /// </summary>
        /// <returns></returns>
        public override string ToEncodedString()
        {
            const string filterPattern = @"{{""type"":""MultiRowRangeFilter"",""ranges"":[{0}]}}";
            return string.Format(CultureInfo.InvariantCulture, filterPattern, RowRangeToCsvString());
        }
        /// <summary>
        ///rowKeyPrefixes - the array of byte array
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            const string filterPattern = @"{{""type"":""MultiRowRangeFilter"",""rowKeyPrefixes"":[{0}]}}";
            return string.Format(CultureInfo.InvariantCulture, filterPattern, RowRangeToCsvString());
        }
        private string RowRangeToCsvString()
        {
            if (rangeList.Count == 0)
            {
                return string.Empty;
            }

            var working = new StringBuilder();
            foreach (Filter f in rangeList)
            {
                working.AppendFormat(@"{0},", f.ToEncodedString());
            }

            // remove the trailing ','
            return working.ToString(0, working.Length - 1);
        }
    }
}
