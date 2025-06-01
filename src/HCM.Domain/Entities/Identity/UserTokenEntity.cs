namespace HCM.Domain.Entities.Identity
{
    public class UserTokenEntity : EntityBase<UserTokenEntity>
    {
        public Guid UserId { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiredAt { get; set; }
    }
}
