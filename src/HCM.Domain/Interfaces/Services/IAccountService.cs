using HCM.Domain.Models.Identity;
using HCM.Domain.ViewModels.Identity;

namespace HCM.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        Task<LoginViewModel> LoginAsync(LoginModel model);

        Task RegisterAsync(UserModel model);

        Task UpdateAccountAsync(UserModel model);

        Task<TokenModel> RefreshTokenAsync(RefreshTokenModel model);

        Task LogoutAsync(RefreshTokenModel model);

        Task<IEnumerable<UserViewModel>> GetAllAsync();

        Task<IEnumerable<RoleViewModel>> GetAllUserRolesAsync();
    }
}
