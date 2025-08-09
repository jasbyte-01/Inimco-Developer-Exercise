namespace ProfileAnalyzer.Application.DTOs
{
    public class UserDTO
    {
        public required string FirstName { get; init; }

        public required string LastName { get; init; }

        public required List<string> SocialSkills { get; init; }

        public required List<SocialAccountDTO> SocialAccounts { get; init; }
    }
}
