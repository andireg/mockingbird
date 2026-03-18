using System.Text.Json;
using System.Text.RegularExpressions;

namespace Mockingbird.Factory.Moq;

internal static class ConvertUtils
{
    internal static object? ConvertToType(object? source, Type targetType)
    {
        if (source == null)
        {
            return null;
        }

        string json = JsonSerializer.Serialize(source);
        Type realTargetType = targetType;
        if (realTargetType.IsByRef)
        {
            realTargetType = Type.GetType(realTargetType.FullName![0..^1])!;
        }

        json = Regex.Replace(
            json,
            @"""\$Type""\:""([^""]*)"",",
            match =>
            {
                realTargetType = Type.GetType(match.Groups[1].Value)!;
                return string.Empty;
            });

        return JsonSerializer.Deserialize(json, realTargetType);
    }
}