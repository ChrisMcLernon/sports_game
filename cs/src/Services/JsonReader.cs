using System.Text.Json;

namespace sports_game.src.Services
{
    public class JsonReader
    {
        private static readonly string BASE_PATH = Path.GetFullPath("C:/CodingDir/sports_game/cs/src/Data/Data.json");

        public static T Read<T>(string key)
        {
        
            string jsonString = File.ReadAllText(BASE_PATH);
            
            JsonDocument document = JsonDocument.Parse(jsonString);
            JsonElement root = document.RootElement;
        
            if (root.ValueKind == JsonValueKind.Object)
            {
                JsonElement.ObjectEnumerator enumerator = root.EnumerateObject();
        
                while (enumerator.MoveNext())
                {
                    JsonProperty property = enumerator.Current;
                    if (property.Name == key)
                    {
                        return JsonSerializer.Deserialize<T>(property.Value, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? throw new InvalidOperationException("Deserialization returned null.");
                    }
                }
            }
        
            throw new KeyNotFoundException($"Key '{key}' not found.");
        }
    }
}