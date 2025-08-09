namespace ProfileAnalyzer.Domain.Interfaces
{
    public interface IFileReader
    {
        Task<T?> ReadJsonFileAsync<T>(string filePath);

        Task WriteJsonFileAsync<T>(string filePath, T content);
    }
}
