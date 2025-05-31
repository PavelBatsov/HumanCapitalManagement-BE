using HCM.Domain.Entities;
using HCM.Domain.Models.Identity;

namespace HCM.Domain.Interfaces.Services
{
    public interface ITokenHandlerService
    {
        Task<TokenModel> GenerateTokenAsync(UserEntity applicationUser);
    }
}
