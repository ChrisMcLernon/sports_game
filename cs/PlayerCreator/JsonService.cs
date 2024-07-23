using System.Text.Json;

static public class JsonService
{
    static private readonly string BASE_PATH = Path.GetFullPath("../src/Data/Data.json");

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

    static public void Write<T>(string key, T value)
    {
        string jsonString = File.ReadAllText(BASE_PATH);
        JsonDocument document = JsonDocument.Parse(jsonString);
        JsonElement root = document.RootElement;

        // Convert existing JSON structure to a mutable dictionary
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);

        if (dictionary == null)
        {
            throw new InvalidOperationException("Failed to deserialize existing JSON data.");
        }

        // Add or update the key-value pair
        dictionary[key] = JsonSerializer.SerializeToElement(value);

        // Serialize updated dictionary back to JSON
        string updatedJsonString = JsonSerializer.Serialize(dictionary, new JsonSerializerOptions { WriteIndented = true });

        // Write updated JSON back to file
        File.WriteAllText(BASE_PATH, updatedJsonString);
    }
}