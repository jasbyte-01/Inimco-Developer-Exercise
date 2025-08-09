using ProfileAnalyzer.Domain.Enums;

namespace ProfileAnalyzer.Domain.Entities
{
    public class SocialAccount
    {
        public int UserId { get; set; }
        public int SocialAccountId { get; set; }
        public required SocialAccountType Type { get; init; }
        public required string Address { get; init; }
    }
}
