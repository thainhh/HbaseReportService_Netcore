using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HbaseReportService.Hbase
{
    public class HbaseOption
    {
        /// <summary>
        /// Rest IP
        /// </summary>
        public string IPAddresses { get; set; } = "http://10.0.10.148";
        /// <summary>
        /// Port Rest
        /// </summary>
        public int Port { get; set; } = 80;
        /// <summary>
        /// Basic user
        /// </summary>
        public string UserName { get; set; } = "hbase";
        /// <summary>
        /// Basic pass
        /// </summary>
        public string Password { get; set; } = "nYA8tvaL3czqYGb6MFhYqtHG";
        public int TimeoutMillis { get; set; } = 300000;
        public bool UseNagle { get; set; }
        public bool KeepAlive { get; set; } = true;
        public int SerializationBufferSize { get; set; } = 1024 * 1024 * 1;
        public int ReceiveBufferSize { get; set; } = 1024 * 1024 * 1;
        public string AlternativeEndpoint { get; set; } = "/hbase-rest/";
        public string IPRestReport { get; set; } = "http://10.0.5.5:8080/api/v1/report";
    }
}
