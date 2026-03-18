using System.Text.Json;

namespace Mockingbird.Invocation;

internal static class InvocationExtensions
{
    public static InvocationInfo? GetInvocation(this TypeInvocationInfo? typeInvocationInfo, string invocationName, object arguments)
        => typeInvocationInfo?.Invocations?.FirstOrDefault(invocation =>
            invocation.InvocationName == invocationName &&
            JsonSerializer.Serialize(invocation.Arguments) == JsonSerializer.Serialize(arguments));

    public static object? ToObject(
        this JsonElement jsonElement)
    {
        return jsonElement.ValueKind switch
        {
            JsonValueKind.Object => jsonElement.EnumerateObject()
                .ToDictionary(
                    property => property.Name, 
                    property => property.Value.ToObject()),
            JsonValueKind.Array => jsonElement.EnumerateArray()
                .Select(
                        element => element.ToObject())
                .ToArray(),
            JsonValueKind.String => jsonElement.GetString(),
            JsonValueKind.Number => jsonElement.GetDecimal(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            _ => null
        };
    }

    public static object? UnpackJsonObject(
        this object value)
    {
        if (value is JsonElement jsonElement)
        {
            return jsonElement.ToObject();
        }

        return value;
    }
}