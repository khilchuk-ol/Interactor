using Microsoft.IdentityModel.Tokens;

namespace SplitDivider.Domain.Common;

public class JwtIssuerOptions
{
    public string Issuer { get; set; }
        public string Subject { get; set; }
        public string Audience { get; set; }
        public DateTime Expiration => IssuedAt.Add(ValidForInMin);
        public DateTime IssuedAt => DateTime.UtcNow;
        public TimeSpan ValidForInMin { get; set; }
        public DateTime NotBefore => DateTime.UtcNow;
        public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());
        public SigningCredentials SigningCredentials { get; set; }
}