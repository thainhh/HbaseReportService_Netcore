using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HbaseReportService.Hbase.Filters
{
    /// <summary>
    /// Implementation of <see cref="Filter"/> that represents an ordered List of Filters which will be evaluated with a specified boolean operator 
    /// FilterList.Operator.MUST_PASS_ALL (AND) or FilterList.Operator.MUST_PASS_ONE (OR).
    /// </summary>
    public class FilterList : Filter
    {
        private readonly List<Filter> _rowFilters;

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Operator")]
        public enum Operator
        {
            /// <summary>
            /// MUST_PASS_ALL
            /// </summary>
            /// <remarks>
            /// Represents "and".
            /// </remarks>
            MustPassAll = 0,

            /// <summary>
            /// MUST_PASS_ONE
            /// </summary>
            /// <remarks>
            /// Represents "or".
            /// </remarks>
            MustPassOne = 1,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterList"/> class.
        /// </summary>
        /// <param name="rowFilters">The row filters.</param>
        public FilterList(params Filter[] rowFilters) : this(Operator.MustPassAll, rowFilters.ToList())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterList"/> class.
        /// </summary>
        /// <param name="op">The op.</param>
        public FilterList(Operator op) : this(op, new List<Filter>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterList"/> class.
        /// </summary>
        /// <param name="op">The op.</param>
        /// <param name="rowFilters">The row filters.</param>
        public FilterList(Operator op, params Filter[] rowFilters) : this(op, rowFilters.ToList())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterList"/> class.
        /// </summary>
        /// <param name="op">The op.</param>
        /// <param name="rowFilters">The row filters.</param>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">op</exception>
        public FilterList(Operator op, IEnumerable<Filter> rowFilters)
        {
            if (!Enum.IsDefined(typeof(Operator), op))
            {
                throw new InvalidEnumArgumentException("op", (int)op, typeof(Operator));
            }


            _rowFilters = new List<Filter>();
            foreach (Filter f in rowFilters)
            {
                if (ReferenceEquals(f, null))
                {
                    throw new Exception("rowFilters null");
                }

                _rowFilters.Add(f);
            }

            Op = op;
        }

        /// <summary>
        /// Gets the filters.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        public IEnumerable<Filter> Filters
        {
            get { return _rowFilters; }
        }

        /// <summary>
        /// Gets the op.
        /// </summary>
        /// <value>
        /// The op.
        /// </value>
        public Operator Op { get; private set; }

        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public void AddFilter(Filter filter)
        {
            _rowFilters.Add(filter);
        }


        /// <inheritdoc/>
        public override string ToEncodedString()
        {
            const string filterPattern = @"{{""type"":""FilterList"",""op"":""{0}"",""filters"":[{1}]}}";
            return string.Format(CultureInfo.InvariantCulture, filterPattern, Op.ToCodeName(), FiltersToCsvString());
        }

        private string FiltersToCsvString()
        {
            if (_rowFilters.Count == 0)
            {
                return string.Empty;
            }

            var working = new StringBuilder();
            foreach (Filter f in _rowFilters)
            {
                working.AppendFormat(@"{0},", f.ToEncodedString());
            }

            // remove the trailing ','
            return working.ToString(0, working.Length - 1);
        }
    }
}
