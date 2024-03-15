using HbaseReportService.Hbase.Generated;
using ProtoBuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HbaseReportService.Hbase
{
    public class HbaseParser : IHbaseParser
    {
        public Dictionary<string, string> GetPropertyNameWithFamilyStr<T>() where T : class
        {
            var hcaType = typeof(HbaseColumnAttribute);

            var properties = typeof(T).GetProperties()
                            .ToList();

            var result = new Dictionary<string, string>();

            foreach (var property in properties)
            {
                var att = property.GetCustomAttribute(hcaType) as HbaseColumnAttribute;
                if (att == null) continue;
                var family = att.Family;
                var column = att.Column;
                if (string.IsNullOrWhiteSpace(family))
                {
                    family = HbaseColumnAttribute.DefaultFamily;
                }
                if (string.IsNullOrWhiteSpace(column))
                {
                    column = property.Name;
                }
                result.Add(property.Name, $"{family}:{column}");
            }
            return result;
        }
        public Dictionary<string, byte[]> GetPropertyNameWithFamilyBytes<T>() where T : class
        {
            var result = new Dictionary<string, byte[]>();
            var strDic = GetPropertyNameWithFamilyStr<T>();
            foreach (var item in strDic)
            {
                result.Add(item.Key, item.Value.ToBytes());
            }
            return result;
        }
        public T ToReal<T>(CellSet.Row trr) where T : class, IHbaseTable, new()
        {
            var real = new T
            {
                RowKey = trr.key.ToObject<string>()
            };

            var nameWithFamily = GetPropertyNameWithFamilyStr<T>();
            var properties = typeof(T).GetProperties()
                .Where(t => nameWithFamily.ContainsKey(t.Name))
                .ToList();

            var dict = trr.values.ToDictionary(t => t.column.ToObject<string>());

            foreach (var property in properties)
            {
                if (dict.TryGetValue(nameWithFamily[property.Name], out var tCell))
                {
                    object v = tCell.data.ToObject(property.PropertyType);
                    property.SetValue(real, v);
                }
            }
            return real;
        }

        public List<T> ToReals<T>(IEnumerable<CellSet.Row> trrs) where T : class, IHbaseTable, new()
        {

            var nameWithFamily = GetPropertyNameWithFamilyStr<T>();
            var properties = typeof(T).GetProperties()
                .Where(t => nameWithFamily.ContainsKey(t.Name))
                .ToList();

            var result = new List<T>();
            foreach (var trr in trrs)
            {
                var real = new T
                {
                    RowKey = trr.key.ToObject<string>()
                };
                var dict = trr.values.ToDictionary(t => t.column.ToObject<string>());
                foreach (var property in properties)
                {
                    if (dict.TryGetValue(nameWithFamily[property.Name], out var tCell))
                    {
                        object v = tCell.data.ToObject(property.PropertyType);
                        property.SetValue(real, v);
                    }
                }
                result.Add(real);
            }
            return result;
        }
    }
}
