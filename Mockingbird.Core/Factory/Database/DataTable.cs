using Mockingbird.Utils;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace Mockingbird.Factory.Database
{
    internal class DataTable
    {
        public DataTable(object? data)
        {
            if (data is not IEnumerable enumerable)
            {
                return;
            }

            object[] array = enumerable.Cast<object>().ToArray();
            if (array.Length < 2)
            {
                return;
            }

            Columns = ((IEnumerable)array[0]).Cast<JValue>().Select(item => (string)item.Value!).ToArray();
            Types = ((IEnumerable)array[1]).Cast<JValue>()
                .Select(item => TypeUtils.GetTypeByFriendlyName((string)item.Value!) ?? Type.GetType((string)item.Value!)!)
                .ToArray();
            Data = array.Skip(2)
                .Select(row => ((IEnumerable)row)
                    .Cast<JValue>()
                    .Select((item, idx) => ObjectConverter.ConvertObject(item.Value, Types[idx]))
                    .ToArray())
                .ToArray();
        }

        public string[] Columns { get; } = Array.Empty<string>();

        public Type[] Types { get; } = Array.Empty<Type>();

        public object?[][] Data { get; } = Array.Empty<string[]>();
    }
}