using HCM.Domain.Entities.Identity;

namespace HCM.Domain.Interfaces.Repositories
{
    public interface IUserTokenRepository : IGenericRepository<UserTokenEntity>
    {
        Task<UserTokenEntity> GetRefreshTokenAsync(string refreshToken);
    }
}
