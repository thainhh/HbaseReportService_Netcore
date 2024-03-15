using System.ComponentModel;

namespace HbaseReportService.Hbase.Filters
{
    internal static class ObjectExtensions
    {
        /// <summary>
        /// Evaluates type compatibility.
        /// </summary>
        /// <typeparam name = "T">
        /// The type to evaluate against.
        /// </typeparam>
        /// <param name = "inputValue">
        /// The object to evaluate compatibility for.
        /// </param>
        /// <returns>
        /// True if the object is compatible otherwise false.
        /// </returns>
        internal static bool Is<T>(this object inputValue)
        {
            return inputValue is T;
        }

        /// <summary>
        /// Determines whether the specified object is not null.
        /// </summary>
        /// <param name = "inputValue">The object.</param>
        /// <returns>
        /// <c>true</c> if the specified object is not null; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsNotNull([ValidatedNotNull] this object inputValue)
        {
            return !ReferenceEquals(inputValue, null);
        }

        /// <summary>
        /// Determines whether the specified object is null.
        /// </summary>
        /// <param name = "inputValue">The object.</param>
        /// <returns>
        /// <c>true</c> if the specified object is null; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsNull([ValidatedNotNull] this object inputValue)
        {
            return ReferenceEquals(inputValue, null);
        }

        internal static string ToCodeName(this BitComparator.BitwiseOp value)
        {
            switch (value)
            {
                case BitComparator.BitwiseOp.And:
                    return "AND";

                case BitComparator.BitwiseOp.Or:
                    return "OR";

                case BitComparator.BitwiseOp.Xor:
                    return "XOR";

                default:
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(BitComparator.BitwiseOp));
            }
        }

        internal static string ToCodeName(this CompareFilter.CompareOp value)
        {
            switch (value)
            {
                case CompareFilter.CompareOp.NoOperation:
                    return "NO_OP";

                case CompareFilter.CompareOp.Equal:
                    return "EQUAL";

                case CompareFilter.CompareOp.NotEqual:
                    return "NOT_EQUAL";

                case CompareFilter.CompareOp.GreaterThan:
                    return "GREATER";

                case CompareFilter.CompareOp.GreaterThanOrEqualTo:
                    return "GREATER_OR_EQUAL";

                case CompareFilter.CompareOp.LessThan:
                    return "LESS";

                case CompareFilter.CompareOp.LessThanOrEqualTo:
                    return "LESS_OR_EQUAL";

                default:
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(CompareFilter.CompareOp));
            }
        }

        internal static string ToCodeName(this FilterList.Operator value)
        {
            switch (value)
            {
                case FilterList.Operator.MustPassAll:
                    return "MUST_PASS_ALL";

                case FilterList.Operator.MustPassOne:
                    return "MUST_PASS_ONE";

                default:
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(FilterList.Operator));
            }
        }
    }
}
