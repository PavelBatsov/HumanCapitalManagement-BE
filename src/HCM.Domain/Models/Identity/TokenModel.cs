namespace HCM.Domain.Models.Identity
{
    public class TokenModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiredAt { get; set; }
    }
}
