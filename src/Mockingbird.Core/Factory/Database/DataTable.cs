using Mockingbird.Utils;
using System.Collections;

namespace Mockingbird.Factory.Database;

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

// TODO: Refactor to use System.Text.Json instead of Newtonsoft.Json to avoid dependency on Newtonsoft.Json
        Columns = ((IEnumerable)array[0]).Cast<object>().Select(item => (string)item!).ToArray();
        Types = ((IEnumerable)array[1]).Cast<object>()
            .Select(item => TypeUtils.GetTypeByFriendlyName((string)item!) ?? Type.GetType((string)item!)!)
            .ToArray();
        Data = array.Skip(2)
            .Select(row => ((IEnumerable)row)
                .Cast<object>()
                .Select((item, idx) => ObjectConverter.ConvertObject(item, Types[idx]))
                .ToArray())
            .ToArray();
    }

    public string[] Columns { get; } = Array.Empty<string>();

    public Type[] Types { get; } = Array.Empty<Type>();

    public object?[][] Data { get; } = Array.Empty<string[]>();
}