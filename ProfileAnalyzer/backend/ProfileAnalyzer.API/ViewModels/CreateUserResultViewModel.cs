using ProfileAnalyzer.Application.DTOs;

namespace ProfileAnalyzer.API.ViewModels
{
    public class CreateUserResultViewModel
    {
        public required int Vowels { get; init; }

        public required int Consonants { get; init; }

        public required string ReversedName { get; init; }

        public required UserDTO OriginalData { get; init; }
    }
}
