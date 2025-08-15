using ProfileAnalyzer.Domain.Interfaces;

namespace ProfileAnalyzer.Application.Services
{

    public class NameAnalysisService : INameAnalysisService
    {
        private static readonly List<char> vowels = ['a', 'e', 'i', 'o', 'u', 'y'];

        public int CountVowels(string name)
        {
            return name.ToLower().Count(vowels.Contains);
        }

        public int CountConstants(string name)
        {
            // Check if it is a letter and if it is a letter and not occuring in the vowels is it a constant
            return name.ToLower().Count(c => char.IsLetter(c) && !vowels.Contains(c));
        }

        public string ReverseName(string name)
        {
            // Split the name by spaces and reverse each word
            List<string> words =
            [
                .. name.Split(' ').Select(word => new string([.. word.Reverse()])),
            ];
            return string.Join(" ", words);
        }
    }
}
