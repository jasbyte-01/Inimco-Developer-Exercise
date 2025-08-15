using ProfileAnalyzer.Domain.Interfaces;

namespace ProfileAnalyzer.Application.Services
{
    public class FileReader : IFileReader
    {
        private static readonly System.Text.Json.JsonSerializerOptions CachedJsonOptions =
            new System.Text.Json.JsonSerializerOptions { WriteIndented = true };

        public async Task<T?> ReadJsonFileAsync<T>(string filePath)
        {
            // If file exists return the content other wise return null
            if (File.Exists(filePath))
            {
                string? jsonContent = await File.ReadAllTextAsync(filePath);
                T? result = System.Text.Json.JsonSerializer.Deserialize<T>(jsonContent);
                return result;
            }
            else
            {
                return default;
            }
        }

        public async Task WriteJsonFileAsync<T>(string filePath, T content)
        {
            // Create the folder if it does not exist
            string? directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Convert the data into a JSON string
            string jsonContent = System.Text.Json.JsonSerializer.Serialize(
                content,
                CachedJsonOptions
            );

            await File.WriteAllTextAsync(filePath, jsonContent);
        }
    }
}
