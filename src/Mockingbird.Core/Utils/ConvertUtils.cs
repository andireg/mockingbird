using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Mockingbird.Factory.Moq
{
    internal static class ConvertUtils
    {
        internal static object? ConvertToType(object? source, Type targetType)
        {
            if (source == null)
            {
                return null;
            }

            string json = JsonConvert.SerializeObject(source);
            Type realTargetType = targetType;

            json = Regex.Replace(
                json,
                @"""\$Type""\:""([^""]*)"",",
                match =>
                {
                    realTargetType = Type.GetType(match.Groups[1].Value)!;
                    return string.Empty;
                });

            return JsonConvert.DeserializeObject(json, realTargetType);
        }
    }
}