using System.Text.Json;
using Mockingbird.Invocation;

namespace Mockingbird.Utils;

internal static class ObjectConverter
{
    public static Dictionary<string, object?>? ConvertToDictionary(object? source)
    {
        if (source == null)
        {
            return null;
        }

        string json = JsonSerializer.Serialize(source);
        try
        {
            Dictionary<string, object?>? dictionary = JsonSerializer.Deserialize<Dictionary<string, object?>>(json);
            return dictionary
                ?.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.UnpackJsonObject());
        }
        catch
        {
            return null;
        }
    }

    public static T? ConvertObject<T>(object? source)
    {
        if (source == null)
        {
            return default;
        }

        string json = JsonSerializer.Serialize(source);
        try
        {
            return JsonSerializer.Deserialize<T>(json);
        }
        catch
        {
            return default;
        }
    }

    internal static object? ConvertObject(object? source, Type targetType)
    {
        if (source == null)
        {
            return null;
        }

        string json = JsonSerializer.Serialize(source);
        try
        {
            return JsonSerializer.Deserialize(json, targetType);
        }
        catch
        {
            return null;
        }
    }
}