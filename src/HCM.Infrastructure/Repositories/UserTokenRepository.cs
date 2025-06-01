using HCM.Domain.Entities.Identity;
using HCM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HCM.Infrastructure.Repositories
{
    public class UserTokenRepository : GenericRepository<UserTokenEntity>, IUserTokenRepository
    {
        private readonly AppDbContext context;

        public UserTokenRepository(AppDbContext context)
            : base(context)
        { 
            this.context = context;
        }

        public async Task<UserTokenEntity> GetRefreshTokenAsync(string refreshToken)
        {
            var userToken = await context.Set<UserTokenEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            return userToken;
        }
    }
}
