using Elasticsearch.Net;
using NPOI.SS.Formula.Eval;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HbaseReportService.Hbase.Filters
{
    public class RowRange : Filter
    {
        private byte[] start_row;
        private bool start_row_inclusive;
        private byte[] stop_row;
        private bool stop_row_inclusive;
        /// <summary>
        /// Trả về xem hàng giới hạn được sử dụng cho tìm kiếm nhị phân có bao gồm hay không. 
        /// Để quét tiến, chúng tôi sẽ kiểm tra starRow, nhưng chúng tôi sẽ kiểm tra stopRow đối với trường hợp quét ngược.
        /// </summary>
        /// <param name="_startRow"></param>
        /// <param name="_startRowInclusive"></param>
        /// <param name="_stopRow"></param>
        /// <param name="_stopRowInclusive"></param>
        public RowRange(String _startRow, bool _startRowInclusive, String _stopRow,
          bool _stopRowInclusive)
        {
            this.start_row = Encoding.UTF8.GetBytes(_startRow);
            this.start_row_inclusive = _startRowInclusive;
            this.stop_row = Encoding.UTF8.GetBytes(_stopRow);
            this.stop_row_inclusive = _stopRowInclusive;
        }

        public override string ToEncodedString()
        {
            const string filterPattern = @"{{""startRow"":""{0}"",""startRowInclusive"":{1},""stopRow"":""{2}"",""stopRowInclusive"":{3}}}";
            return string.Format(CultureInfo.InvariantCulture, filterPattern, Convert.ToBase64String(start_row), start_row_inclusive.ToString(CultureInfo.InvariantCulture).ToLowerInvariant()
                , Convert.ToBase64String(stop_row), stop_row_inclusive.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
        }
    }
}
