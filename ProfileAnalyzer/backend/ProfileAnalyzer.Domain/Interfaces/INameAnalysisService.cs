namespace ProfileAnalyzer.Domain.Interfaces
{
    public interface INameAnalysisService
    {
        int CountVowels(string name);
        int CountConstants(string name);
        string ReverseName(string name);
    }
}
