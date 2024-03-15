using System.Globalization;

namespace HbaseReportService.Hbase.Filters
{
    /// <summary>
    /// Implementation of Filter interface that limits results to a specific page size.
    /// </summary>
    /// <remarks>
    /// Note that this filter cannot guarantee that the number of results returned to a client are less than or equal to page size. This is because 
    /// the filter is applied separately on different region servers. It does however optimize the scan of individual HRegions by making sure that 
    /// the page size is never exceeded locally.
    /// </remarks>
    public class PageFilter : Filter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageFilter"/> class.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        public PageFilter(long pageSize)
        {
            PageSize = pageSize;
        }

        /// <summary>
        /// Gets the maximum size of a page.
        /// </summary>
        /// <value>
        /// The maximum size of a page.
        /// </value>
        public long PageSize { get; private set; }

        /// <inheritdoc/>
        public override string ToEncodedString()
        {
            const string filterPattern = @"{{""type"":""PageFilter"",""value"":""{0}""}}";
            return string.Format(CultureInfo.InvariantCulture, filterPattern, PageSize);
        }
    }
}
