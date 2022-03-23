using Mockingbird.Invocation;
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

        internal static string SerializeObject(object? instance, Formatting formatting) => JsonConvert.SerializeObject(instance, formatting, settings);

        public static T? DeserializeObject<T>(string text) => JsonConvert.DeserializeObject<T>(text, settings);

        public static object? DeserializeObject(string text) => JsonConvert.DeserializeObject(text, settings);
    }
}
