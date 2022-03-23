using Newtonsoft.Json;

namespace Mockingbird.Utils
{
    internal static class JsonUtils
    {
        private static readonly JsonSerializerSettings settings = new()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };

        public static string SerializeObject(object? instance) => JsonConvert.SerializeObject(instance, settings);

        public static T? SerializeObject<T>(string text) => JsonConvert.DeserializeObject<T>(text, settings);

        public static object? SerializeObject(string text) => JsonConvert.DeserializeObject(text, settings);
    }
}
