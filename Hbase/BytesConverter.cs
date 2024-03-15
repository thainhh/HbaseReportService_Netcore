using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HbaseReportService.Hbase
{
    public static class BytesConverter
    {
        /// <summary>
        /// Nó cần phải tương ứng với java và byte mạng nên đầu to và đầu nhỏ bị đảo ngược.
        /// </summary>
        /// <param name="@this"></param>
        /// <returns></returns>
        public static byte[] ReverseBytes(this byte[] @this)
        {
            Array.Reverse(@this);
            return @this;
        }
        public static byte[] ToBytes(this object obj)
        {
            if (obj == null)
                return null;
            switch (obj)
            {
                case int @int:
                    return BitConverter.GetBytes(@int).ReverseBytes();
                case string @string:
                    return Encoding.UTF8.GetBytes(@string);
                case double @double:
                    return BitConverter.GetBytes(@double).ReverseBytes();
                case long @long:
                    return BitConverter.GetBytes(@long).ReverseBytes();
                case bool @bool:
                    return BitConverter.GetBytes(@bool);
                case float @float:
                    return BitConverter.GetBytes(@float).ReverseBytes();
                // Trong Java, char sử dụng mã Unicode, có bốn byte. nhưng trong csharp nó là hai byte
                case char @char:
                    return BitConverter.GetBytes(@char);
                case short @short:
                    return BitConverter.GetBytes(@short).ReverseBytes();
                default:
                    return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
            }
        }
        public static T ToObject<T>(this byte[] bits)
        {
            return (T)bits.ToObject(typeof(T));
        }
        public static object ToObject(this byte[] bits, Type type)
        {
            if (bits?.Length < 1)
                return null;
            Type ogri;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                ogri = Nullable.GetUnderlyingType(type);
            else ogri = type;
            switch (ogri.Name)
            {
                case nameof(Int32):
                    return BinaryPrimitives.ReadInt32BigEndian(bits);
                case nameof(String):
                    return Encoding.UTF8.GetString(bits);
                case nameof(Double):
                    return BinaryPrimitives.ReadDoubleBigEndian(bits);
                case nameof(Int64):
                    return BinaryPrimitives.ReadInt64BigEndian(bits);
                case nameof(Boolean):
                    return BitConverter.ToBoolean(bits, 0);
                case nameof(Single):
                    return BitConverter.ToSingle(bits, 0);
                case nameof(Char):
                    return BitConverter.ToChar(bits, 0);
                case nameof(Int16):
                    return BitConverter.ToInt16(bits, 0);
                case nameof(DateTime):
                    return DateTimeOffset.FromUnixTimeSeconds((long)BinaryPrimitives.ReadDoubleBigEndian(bits)).DateTime;
                default:
                    var str = Encoding.UTF8.GetString(bits);
                    if (string.IsNullOrWhiteSpace(str)) return null;
                    return JsonSerializer.Deserialize(str, type);
            }
        }
        #region objec

        // public static string ToUTF8String(this byte[] @this)
        // {
        //     return Encoding.UTF8.GetString(@this);
        // }

        #endregion
    }
}
