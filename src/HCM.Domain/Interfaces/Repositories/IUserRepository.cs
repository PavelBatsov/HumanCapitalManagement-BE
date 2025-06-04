using HCM.Domain.Entities;
using HCM.Domain.Entities.Identity;
using HCM.Domain.Models.Identity;

namespace HCM.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<UserEntity>
    {
        Task<RoleModel> GetUserRoleAsync(Guid userId);

        Task<bool> IsUserExistsAsync(string email);

        Task<UserEntity> GetUserByEmailAsync(string email);

        Task<IEnumerable<RoleEntity>> GetAllUserRolesAsync();
    }
}
