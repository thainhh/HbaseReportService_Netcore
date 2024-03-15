using HbaseReportService.Hbase.Generated;
using System;
using System.Diagnostics.CodeAnalysis;

namespace HbaseReportService.Hbase.Filters
{
    public abstract class ByteArrayComparable
    {
        private readonly byte[] _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteArrayComparable"/> class.
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
        protected ByteArrayComparable(byte[] value)
        {

            _value = (byte[])value.Clone();
        }

        /// <summary>
        /// Gets the value to compare.
        /// </summary>
        /// <value>
        /// The value to compare.
        /// </value>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public byte[] Value
        {
            get { return (byte[])_value.Clone(); }
        }

        /// <summary>
        /// Generates an encoded string that can be used to as part of a <see cref="Scanner.filter"/>.
        /// </summary>
        /// <returns>
        /// A comparer string.
        /// </returns>
        public abstract string ToEncodedString();
    }
}
