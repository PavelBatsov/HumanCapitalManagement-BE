using HCM.Domain.Entities;
using HCM.Domain.Entities.Identity;
using HCM.Domain.Interfaces.Repositories;
using HCM.Domain.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace HCM.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<UserEntity>, IUserRepository
    {
        private readonly AppDbContext context;

        public UserRepository(AppDbContext context)
            : base(context)
        {
            this.context = context;
        }

        public async Task<RoleModel> GetUserRoleAsync(Guid userId)
        {
            var appRole = await context.Set<RoleEntity>()
                .AsNoTracking()
                .Join(
                    context.Set<UserRoleEntity>().AsNoTracking(),
                    r => r.Id,
                    ur => ur.RoleId,
                    (r, ur) => new RoleModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        UserId = ur.UserId
                    })
                .Where(x => x.UserId == userId)
                .Select(x => new RoleModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .FirstOrDefaultAsync();

            return appRole;
        }
    }
}
