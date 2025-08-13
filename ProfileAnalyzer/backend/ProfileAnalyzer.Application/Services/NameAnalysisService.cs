namespace ProfileAnalyzer.Application.Services
{
    public interface INameAnalysisService
    {
        int CountVowels(string name);
        int CountConstants(string name);
        string ReverseName(string name);
    }

    public class NameAnalysisService : INameAnalysisService
    {
        private static readonly List<char> vowels = ['a', 'e', 'i', 'o', 'u', 'y'];

        public int CountVowels(string name)
        {
            return name.ToLower().Count(vowels.Contains);
        }

        public int CountConstants(string name)
        {
            return name.ToLower().Count(c => char.IsLetter(c) && !vowels.Contains(c));
        }

        public string ReverseName(string name)
        {
            List<string> words =
            [
                .. name.Split(' ').Select(word => new string([.. word.Reverse()])),
            ];
            return string.Join(" ", words);
        }
    }
}
