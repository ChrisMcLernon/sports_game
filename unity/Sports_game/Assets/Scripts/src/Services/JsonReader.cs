using System.Text.Json;

namespace sports_game.src.Services
{
    static public class JsonReader
    {
        static private readonly string BASE_PATH = Path.GetFullPath("../cs/src/Data/Data.json");

        static public T Read<T>(string key)
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