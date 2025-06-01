using HCM.Domain.Models.Identity;
using HCM.Domain.ViewModels.Identity;

namespace HCM.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        Task<LoginViewModel> LoginAsync(LoginModel model);

        Task RegisterAsync(UserModel model);

        Task<UserViewModel> UpdateAccountAsync(UserModel model);
    }
}
