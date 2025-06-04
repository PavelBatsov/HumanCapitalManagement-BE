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

        public async override Task<UserEntity> GetAsync(Guid id)
        {
            var user = await context.Set<UserEntity>()
                .Include(x => x.Roles)
                    .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async override Task<IEnumerable<UserEntity>> GetAllAsync()
        {
            var users = await context.Set<UserEntity>()
                .Include(x => x.Roles)
                    .ThenInclude(x => x.Role)
                .ToListAsync();

            return users;
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            var user = await context.Set<UserEntity>()
                .AsNoTracking()
                .Include(x => x.Roles)
                    .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == email);

            return user;
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

        public async Task<bool> IsUserExistsAsync(string email)
        {
            var isUserExist = await context.Set<UserEntity>()
                .AsNoTracking()
                .AnyAsync(x => x.Email == email);

            return isUserExist;
        }

        public async Task<IEnumerable<RoleEntity>> GetAllUserRolesAsync()
        {
            var roles = await context.Set<RoleEntity>()
                .AsNoTracking()
                .ToListAsync();

            return roles;
        }

        public async Task<RoleEntity> GetRoleAsync(Guid roleId)
        {
            var role = await context.Set<RoleEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == roleId);

            return role;
        }

        public async Task<bool> IsUserInSameRoleAsync(Guid userId, Guid roleId)
        {
            var isUserInSameRole = await context.Set<UserRoleEntity>()
                .AsNoTracking()
                .AnyAsync(x => x.UserId == userId && x.RoleId == roleId);

            return isUserInSameRole;
        }
    }
}
