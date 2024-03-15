using System;
using System.ComponentModel;
using System.Globalization;

namespace HbaseReportService.Hbase.Filters
{
    public class BitComparator : ByteArrayComparable
    {
        /// <summary>
        /// Represents bitwise operations.
        /// </summary>
        public enum BitwiseOp
        {
            /// <summary>
            /// And.
            /// </summary>
            And = 0,

            /// <summary>
            /// Or.
            /// </summary>
            Or = 1,

            /// <summary>
            /// Exclusive or.
            /// </summary>
            Xor = 2,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitComparator"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bitOperator">The bit operator.</param>
        /// <exception cref="InvalidEnumArgumentException">The value of <paramref name="bitOperator"/> is not recognized.</exception>
        public BitComparator(byte[] value, BitwiseOp bitOperator) : base(value)
        {
            if (!Enum.IsDefined(typeof(BitwiseOp), bitOperator))
            {
                throw new InvalidEnumArgumentException("bitOperator", (int)bitOperator, typeof(BitwiseOp));
            }

            BitOperator = bitOperator;
        }

        /// <summary>
        /// Gets the bit operator.
        /// </summary>
        /// <value>
        /// The bit operator.
        /// </value>
        public BitwiseOp BitOperator { get; private set; }

        /// <inheritdoc/>
        public override string ToEncodedString()
        {
            const string pattern = @"""type"":""BitComparator"", ""value"":""{0}"", ""op"":""{1}""";
            return string.Format(CultureInfo.InvariantCulture, pattern, Convert.ToBase64String(Value), BitOperator.ToCodeName());
        }
    }
}
