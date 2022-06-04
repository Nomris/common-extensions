using System;
using System.IO;
using System.Text;

namespace Extensions
{
    public static class StreamExtensions
    {
        #region Reading

        public static byte[] ReadArray(this Stream stream, int length)
        {
            byte[] buffer = new byte[length];
            int remainder = length;
            while (remainder > 0) remainder -= stream.Read(buffer, length - remainder, remainder);
            return buffer;
        }

        public static object[] ReadArray(this Stream stream, int length, Func<Stream, object> parser)
        {
            object[] objs = new object[length];

            for (int i = 0; i < length; i++)
                objs[i] = parser(stream);
            return objs;
        }

        public static sbyte ReadSByte(this Stream stream)
        {
            return (sbyte)stream.ReadByte();
        }

        public static ushort ReadUInt16(this Stream stream, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = stream.ReadArray(sizeof(ushort));
            if (reverseOrder.Value) Array.Reverse(buffer);
            return BitConverter.ToUInt16(buffer, 0);
        }

        public static short ReadInt16(this Stream stream, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = stream.ReadArray(sizeof(short));
            if (reverseOrder.Value) Array.Reverse(buffer);
            return BitConverter.ToInt16(buffer, 0);
        }

        public static uint ReadUInt32(this Stream stream, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = stream.ReadArray(sizeof(uint));
            if (reverseOrder.Value) Array.Reverse(buffer);
            return BitConverter.ToUInt32(buffer, 0);
        }

        public static int ReadInt32(this Stream stream, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = stream.ReadArray(sizeof(int));
            if (reverseOrder.Value) Array.Reverse(buffer);
            return BitConverter.ToInt32(buffer, 0);
        }

        public static ulong ReadUInt64(this Stream stream, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = stream.ReadArray(sizeof(ulong));
            if (reverseOrder.Value) Array.Reverse(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }

        public static long ReadInt64(this Stream stream, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = stream.ReadArray(sizeof(long));
            if (reverseOrder.Value) Array.Reverse(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }

        public static float ReadSingle(this Stream stream, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = stream.ReadArray(sizeof(float));
            if (reverseOrder.Value) Array.Reverse(buffer);
            return BitConverter.ToSingle(buffer, 0);
        }

        public static double ReadDouble(this Stream stream, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = stream.ReadArray(sizeof(double));
            if (reverseOrder.Value) Array.Reverse(buffer);
            return BitConverter.ToDouble(buffer, 0);
        }

        public static bool ReadBool(this Stream stream)
        {
            byte[] buffer = stream.ReadArray(sizeof(bool));
            return BitConverter.ToBoolean(buffer, 0);
        }

        public static string ReadString(this Stream stream, Encoding encoding = null, bool zeroTerminate = true, int? length = null)
        {
            encoding = encoding ?? Encoding.ASCII;
        
            if (zeroTerminate)
            {
                string buffer = "";
                if (!encoding.IsSingleByte) throw new ArgumentException("Zero termination only aviable in single byte encoding", nameof(encoding));
                byte c;
                if (length.HasValue)
                {
                    while ((c = (byte)stream.ReadByte()) != '\0' && --length > 0)
                        buffer += (char)c;

                    while (--length > 0) stream.ReadByte();
                }
                else while ((c = (byte)stream.ReadByte()) != '\0')
                    buffer += (char)c;

                return buffer;
            }

            if (!length.HasValue) throw new ArgumentNullException(nameof(length));
            if (length <= 0) return "";

            return encoding.GetString(stream.ReadArray(length.Value));
        }

        public static Guid ReadGuid(this Stream stream)
        {
            return new Guid(stream.ReadArray(16));
        }

#endregion

#region Writer

        public static void WriteFixed(this Stream stream, long? size, byte[] data)
        {
            if (!size.HasValue)
            {
                stream.Write(data);
                return;
            }

            byte[] buffer = new byte[size.Value];
            Array.Copy(data, buffer, Math.Min(buffer.LongLength, data.LongLength));
            stream.Write(buffer);
        }

        public static void Write(this Stream stream, byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
        }

        public static void Write(this Stream stream, byte data)
        {
            stream.Write(new byte[] { data });
        }

        public static void Write(this Stream stream, sbyte data)
        {
            stream.Write(new byte[] { (byte)data });
        }

        public static void Write(this Stream stream, ushort value, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverseOrder.Value) Array.Reverse(buffer);
            stream.Write(buffer);
        }

        public static void Write(this Stream stream, short value, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverseOrder.Value) Array.Reverse(buffer);
            stream.Write(buffer);
        }

        public static void Write(this Stream stream, uint value, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverseOrder.Value) Array.Reverse(buffer);
            stream.Write(buffer);
        }

        public static void Write(this Stream stream, int value, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverseOrder.Value) Array.Reverse(buffer);
            stream.Write(buffer);
        }

        public static void Write(this Stream stream, ulong value, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverseOrder.Value) Array.Reverse(buffer);
            stream.Write(buffer);
        }

        public static void Write(this Stream stream, long value, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverseOrder.Value) Array.Reverse(buffer);
            stream.Write(buffer);
        }

        public static void Write(this Stream stream, float value, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverseOrder.Value) Array.Reverse(buffer);
            stream.Write(buffer);
        }

        public static void Write(this Stream stream, double value, bool? reverseOrder = null)
        {
            if (!reverseOrder.HasValue) reverseOrder = BitConverter.IsLittleEndian;
            byte[] buffer = BitConverter.GetBytes(value);
            if (reverseOrder.Value) Array.Reverse(buffer);
            stream.Write(buffer);
        }

        public static void Write(this Stream stream, bool value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }

        public static void Write(this Stream stream, string s, Encoding encoding = null, bool prependLength = false, int prependSize = sizeof(int))
        {
            encoding = encoding ?? Encoding.ASCII;
            byte[] rawStr = encoding.GetBytes(s);

            if (prependLength)
            {
                byte[] buffer = new byte[prependSize];
                Array.Copy(BitConverter.GetBytes(rawStr.Length), buffer, prependSize);
                if (BitConverter.IsLittleEndian) Array.Reverse(buffer);
                stream.Write(buffer);
            }

            stream.Write(rawStr);

            if (encoding.IsSingleByte && !prependLength) stream.WriteByte(0);
        }

        public static void Write(this Stream stream, Guid guid)
        {
            stream.Write(guid.ToByteArray());
        }

        public static void Write(this Stream stream, IStreamable streamable)
        {
            streamable.ToStream(stream);
        }

        public static void Write<T>(this Stream stream, T @enum) where T : Enum
        {
            Type baseType = typeof(T).GetEnumUnderlyingType();
            if (baseType == typeof(byte))
            {
                Write(stream, (byte)(object)@enum);
                return;
            }
            if (baseType == typeof(sbyte))
            {
                Write(stream, (sbyte)(object)@enum);
                return;
            }
            if (baseType == typeof(ushort))
            {
                Write(stream, (ushort)(object)@enum);
                return;
            }
            if (baseType == typeof(short))
            {
                Write(stream, (short)(object)@enum);
                return;
            }
            if (baseType == typeof(uint))
            {
                Write(stream, (uint)(object)@enum);
                return;
            }
            if (baseType == typeof(int))
            {
                Write(stream, (int)(object)@enum);
                return;
            }
            if (baseType == typeof(ulong))
            {
                Write(stream, (ulong)(object)@enum);
                return;
            }
            if (baseType == typeof(long))
            {
                Write(stream, (long)(object)@enum);
                return;
            }
            throw new InvalidCastException("Unknown Number Value");
        }

        public static T ReadEnum<T>(this Stream stream) where T : Enum
        {
            Type type = typeof(T);
            Type baseType = type.GetEnumUnderlyingType();
            if (baseType == typeof(byte))
                return (T)Enum.ToObject(type, (byte)stream.ReadByte());
            if (baseType == typeof(sbyte))
                return (T)Enum.ToObject(type, stream.ReadSByte());
            if (baseType == typeof(ushort))
                return (T)Enum.ToObject(type, stream.ReadUInt16());
            if (baseType == typeof(short))
                return (T)Enum.ToObject(type, stream.ReadInt16());
            if (baseType == typeof(uint))
                return (T)Enum.ToObject(type, stream.ReadUInt32());
            if (baseType == typeof(int))
                return (T)Enum.ToObject(type, stream.ReadInt32());
            if (baseType == typeof(ulong))
                return (T)Enum.ToObject(type, stream.ReadUInt64());
            if (baseType == typeof(long))
                return (T)Enum.ToObject(type, stream.ReadInt64());

            throw new InvalidCastException("Unknown Number Value");
        }

        public static StreamPackage OpenPackage(this Stream stream)
        {
            return new StreamPackage(stream);
        }

        public static void ClosePackage(this Stream stream, StreamPackage package)
        {
            package.Submit(stream);
        }

#endregion
    
        
    }
}
