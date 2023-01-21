

using Microsoft.IdentityModel.Tokens;

namespace StackOverflow.Solution.Authenticate
{
    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/token";
        public string Issuer { get; set; }
        public string Audience { get; set; }

        public TimeSpan Expiration { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}
