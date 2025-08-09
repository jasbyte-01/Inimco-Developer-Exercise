namespace ProfileAnalyzer.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }

        public required string FirstName { get; init; }

        public required string LastName { get; init; }

        public required List<string> SocialSkills { get; init; }

        public required List<SocialAccount> SocialAccounts { get; init; }
    }
}
