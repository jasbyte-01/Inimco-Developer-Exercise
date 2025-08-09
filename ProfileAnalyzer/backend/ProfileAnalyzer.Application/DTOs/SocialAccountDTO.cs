using ProfileAnalyzer.Domain.Enums;

namespace ProfileAnalyzer.Application.DTOs
{
    public class SocialAccountDTO
    {
        public required SocialAccountType Type { get; init; }

        public required string Address { get; init; }
    }
}
