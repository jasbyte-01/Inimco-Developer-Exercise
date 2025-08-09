using System.Text.Json.Serialization;

namespace ProfileAnalyzer.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SocialAccountType
    {
        Facebook,
        Instagram,
        Twitter,
        LinkedIn
    }
}
