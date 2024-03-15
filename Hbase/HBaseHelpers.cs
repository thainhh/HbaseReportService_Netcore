using HbaseReportService.Hbase.Generated;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HbaseReportService.Hbase
{
    public static class HBaseHelpers
    {
        public const string SplitSchemaCharacter = ".";
        /// <summary>
        /// 1 GPSBA
        /// 2 TCT
        /// 3 GIS
        /// 4 TAXI
        /// 5 XEMAY
        /// </summary>
        public const string HbaseSystemKey = "1";
        public static CellSet.Row GetCellSet(string key, string columnFamily, string[] columnName, string[] value, long timestamp)
        {
            CellSet.Row row = new CellSet.Row() { key = Encoding.UTF8.GetBytes(key) };
            for (var i = 0; i < columnName.Length; i++)
            {
                Cell c1 = new Cell() { column = BuildCellColumn(columnFamily, columnName[i]), row = row.key };
                if (value[i] != null)
                {
                    c1.data = Encoding.UTF8.GetBytes(value[i]);
                }

                if (timestamp > 0)
                {
                    c1.timestamp = timestamp;
                }
                row.values.Add(c1);
            }

            return row;
        }
        public static Byte[] BuildCellColumn(string columnFamilyName, string columnName)
        {
            return Encoding.UTF8.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0}:{1}", columnFamilyName, columnName));
        }
        public static CellSet CreateCellSet(params CellSet.Row[] rows)
        {
            CellSet cellSet = new CellSet();
            cellSet.rows.AddRange(rows);
            return cellSet;
        }
        public static T DeSerialObject<T>(List<CellSet.Row> source)
        {
            var dest = typeof(T);
            //switch (dest)
            //{
            //    case Stop stop:
            //        {
            //            MapStop(stop, source);
            //        }
            //        break;
            //}

            return default;
        }
        public static string ExtractColumnName(Byte[] cellColumn)
        {
            string qualifiedColumnName = Encoding.UTF8.GetString(cellColumn);
            string[] parts = qualifiedColumnName.Split(new[] { ':' }, 2);
            return parts[1];
        }
        //private static void MapStop(Stop stop, CellSet.Row source)
        //{
        //    foreach (var r in source.values)
        //    {
        //        var col = ExtractColumnName(r.column);
        //        switch (col)
        //        {
        //            case "1":
        //                {
        //                    stop.CompanyID = BitConverter.ToInt32(r.data);
        //                }
        //                break;
        //            case "2":
        //                {
        //                    stop.VehicleID = BitConverter.ToInt64(r.data);
        //                }
        //                break;
        //            case "3":
        //                {
        //                    stop.XNCode = Encoding.UTF8.GetString(r.data);
        //                }
        //                break;
        //            default:
        //                {

        //                }
        //                break;
        //        }

        //    }
        //}
    }
}
