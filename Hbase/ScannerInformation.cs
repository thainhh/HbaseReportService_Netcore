using System;
using System.Net;

namespace HbaseReportService.Hbase
{
    [Serializable]
    public sealed class ScannerInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScannerInformation"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="responseHeaderCollection">additional header information from the response</param>
        public ScannerInformation(Uri location, string tableName, WebHeaderCollection responseHeaderCollection)
        {
            Location = location;
            TableName = tableName;
            ResponseHeaderCollection = responseHeaderCollection;
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public Uri Location { get; private set; }

        /// <summary>
        /// Gets the scanner identifier.
        /// </summary>
        /// <value>
        /// The scanner identifier.
        /// </value>
        public string ScannerId
        {
            get { return Location.PathAndQuery.Substring(Location.PathAndQuery.LastIndexOf('/') + 1); }
        }

        /// <summary>
        /// Additional headers from the CreateScanner response.
        /// This can be used to implement a sticky load balancing, e.g. by supplying a remote identifier. 
        /// </summary>
        public WebHeaderCollection ResponseHeaderCollection { get; private set; }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { get; private set; }
    }
}
