namespace HCM.Domain.Configurations
{
    public class AuthenticationConfiguration
    {
        public string Secret { get; set; }

        public long AccessTokenExpiration { get; set; }

        public long RefreshTokenExpiration { get; set; }

        public string Audience { get; set; }

        public string Issuer { get; set; }
    }
}
