namespace HCM.Domain.Models.Identity
{
    public class RefreshTokenModel
    {
        public Guid UserId { get; set; }

        public string RefreshToken { get; set; }
    }
}
