using System.Text.Json;

namespace Mockingbird.Utils
{
    internal static class JsonUtils
    {
        private static readonly JsonSerializerOptions settings = new()
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
        };

        private static readonly JsonSerializerOptions settingsIndent = new()
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
            WriteIndented = true,
        };

        public static string SerializeObject(object? instance) 
            => JsonSerializer.Serialize(instance, settings);

        internal static string SerializeObjectIndent(object? instance) 
            => JsonSerializer.Serialize(instance, settingsIndent);

        public static T? DeserializeObject<T>(string text) 
            => JsonSerializer.Deserialize<T>(text, settings);
    }
}
