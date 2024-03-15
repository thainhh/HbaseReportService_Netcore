using System;
using System.Globalization;

namespace HbaseReportService.Hbase.Filters
{
    public class BinaryComparator : ByteArrayComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryComparator"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public BinaryComparator(byte[] value) : base(value)
        {
        }

        /// <inheritdoc/>
        public override string ToEncodedString()
        {
            const string pattern = @"""type"":""BinaryComparator"",""value"":""{0}""";
            return string.Format(CultureInfo.InvariantCulture, pattern, Convert.ToBase64String(Value));
        }
    }
}
