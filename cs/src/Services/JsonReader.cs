using System.Security;
using System.Text.Json;

namespace sports_game.src.Services
{
    public class JsonReader
    {
        private static readonly string BASE_PATH = Path.GetFullPath("C:/CodingDir/sports_game/cs/src/Data");

        private static void ValidateBasePath(string relativePath)
        {
            string fullPath = Path.GetFullPath(Path.Combine(BASE_PATH, relativePath));

            if (!fullPath.StartsWith(BASE_PATH, StringComparison.OrdinalIgnoreCase))
            {
                throw new SecurityException($"Base path must be within {BASE_PATH}.");
            }
        }

        public static T Read<T>(string relativePath)
        {
            ValidateBasePath(relativePath);

            string fullPath = Path.GetFullPath(Path.Combine(BASE_PATH, relativePath));

            string jsonString = File.ReadAllText(fullPath);
            return JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? throw new InvalidOperationException("Deserialization returned null.");
        }
    }
}