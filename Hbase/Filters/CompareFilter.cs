using System;
using System.ComponentModel;

namespace HbaseReportService.Hbase.Filters
{
    public abstract class CompareFilter : Filter
    {
        /// <summary>
        /// Represents comparison operators.
        /// </summary>
        public enum CompareOp
        {
            /// <summary>
            /// No operation.
            /// </summary>
            NoOperation = 0,

            /// <summary>
            /// Equals.
            /// </summary>
            Equal = 1,

            /// <summary>
            /// Not equal.
            /// </summary>
            NotEqual = 2,

            /// <summary>
            /// Greater than.
            /// </summary>
            GreaterThan = 3,

            /// <summary>
            /// Greater than or equal to.
            /// </summary>
            GreaterThanOrEqualTo = 4,

            /// <summary>
            /// Less than.
            /// </summary>
            LessThan = 5,

            /// <summary>
            /// Less than or equal to.
            /// </summary>
            LessThanOrEqualTo = 6,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareFilter"/> class.
        /// </summary>
        /// <param name="compareOp">The compare op.</param>
        /// <param name="comparator">The comparator.</param>
        protected CompareFilter(CompareOp compareOp, ByteArrayComparable comparator)
        {
            if (!Enum.IsDefined(typeof(CompareOp), compareOp))
            {
                throw new InvalidEnumArgumentException("compareOp", (int)compareOp, typeof(CompareOp));
            }
            CompareOperation = compareOp;
            Comparator = comparator;
        }

        /// <summary>
        /// Gets the comparator.
        /// </summary>
        /// <value>
        /// The comparator.
        /// </value>
        public ByteArrayComparable Comparator { get; private set; }

        /// <summary>
        /// Gets the compare operation.
        /// </summary>
        /// <value>
        /// The compare operation.
        /// </value>
        public CompareOp CompareOperation { get; private set; }
    }
}
