using HCM.Domain.Entities;
using HCM.Domain.Entities.Identity;
using HCM.Domain.Helpers;
using HCM.Domain.Interfaces.Repositories;
using HCM.Domain.Interfaces.Services;
using HCM.Domain.Localization;
using HCM.Domain.Models.Identity;
using HCM.Domain.ViewModels.Identity;

namespace HCM.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandlerService tokenHandlerService;

        public AccountService(
            IUserRepository userRepository,
            ITokenHandlerService tokenHandlerService)
        {
            this.userRepository = userRepository;
            this.tokenHandlerService = tokenHandlerService;
        }

        public async Task<LoginViewModel> LoginAsync(LoginModel model)
        {
            var user = await userRepository.GetUserByEmailAsync(model.Email)
                ?? throw new Exception(Strings.UserNotFound);

            var isPasswordValid = PasswordHelper.VerifyPassword(model.Password, user.Password);

            if (!isPasswordValid)
            {
                throw new Exception(Strings.InvalidCredentials);
            }

            var token = tokenHandlerService.GenerateTokenAsync(user);
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleName = user.Roles.FirstOrDefault()?.Role.Name ?? string.Empty
            };

            return new LoginViewModel
            {
                User = userViewModel,
                Token = await token
            };
        }

        public async Task RegisterAsync(UserModel model)
        {
            var isUserExists = await userRepository.IsUserExistsAsync(model.Email);

            if (isUserExists)
            {
                throw new Exception(Strings.UserAlreadyExist);
            }

            var userId = Guid.NewGuid();
            var user = new UserEntity
            {
                Id = userId,
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = PasswordHelper.HashPassword(model.Password),
                Roles =
                [
                    new UserRoleEntity
                    {
                        UserId = userId,
                        RoleId = model.RoleId
                    }
                ],
                CreatedById = userId,
                CreatedOn = DateTime.UtcNow
            };

            await userRepository.AddAsync(user);
            await userRepository.SaveAsync();
        }

        public async Task<UserViewModel> UpdateAccountAsync(UserModel model)
        {
            throw new NotImplementedException();
        }

        public async Task LogoutAsync(RefreshTokenModel model)
        {
            await tokenHandlerService.RevokeRefreshTokenAsync(model);
        }

        public async Task<TokenModel> RefreshTokenAsync(RefreshTokenModel model)
        {
            return await tokenHandlerService.RefreshTokenAsync(model);
        }
    }
}
