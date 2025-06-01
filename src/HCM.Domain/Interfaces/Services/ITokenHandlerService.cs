using HCM.Domain.Entities;
using HCM.Domain.Models.Identity;
using HCM.Domain.ViewModels.Identity;

namespace HCM.Domain.Interfaces.Services
{
    public interface ITokenHandlerService
    {
        Task<TokenModel> GenerateTokenAsync(UserEntity applicationUser);

        Task RevokeRefreshTokenAsync(RefreshTokenModel model);

        Task<TokenModel> RefreshTokenAsync(RefreshTokenModel model);
    }
}
