using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Extensions
{
    public static class IDbCommandExtensions
    {
        public static void AddGenericParameter(this IDbCommand command, string name, object value)
        {
            IDataParameter parameter = command.Parameters.Contains(name) ? (IDataParameter)command.Parameters[name] : command.CreateParameter();
                parameter.ParameterName = name;
            if (value.GetType() == typeof(Guid))
            {

                parameter.Value = ((Guid)value).ToByteArray();
            }
            else
                parameter.Value = value;

            if (command.Parameters.Contains(name)) return;
            command.Parameters.Add(parameter);
        }

        public static void AddStream(this IDbCommand command, string name, Stream stream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);

                AddGenericParameter(command, name, memoryStream.ToArray());
                memoryStream.Close();
            }
        }
    }
}
