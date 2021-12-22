using Newtonsoft.Json;

namespace Mockingbird.Utils
{
    internal static class ObjectConverter
    {
        public static T? ConvertObject<T>(object? source)
        {
            if (source == null)
            {
                return default;
            }

            string json = JsonConvert.SerializeObject(source);
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
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

            string json = JsonConvert.SerializeObject(source);
            try
            {
                return JsonConvert.DeserializeObject(json, targetType);
            }
            catch
            {
                return null;
            }
        }
    }
}
