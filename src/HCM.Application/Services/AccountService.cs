using HCM.Domain.Entities;
using HCM.Domain.Entities.Identity;
using HCM.Domain.Helpers;
using HCM.Domain.Interfaces;
using HCM.Domain.Interfaces.Repositories;
using HCM.Domain.Interfaces.Services;
using HCM.Domain.Localization;
using HCM.Domain.Models.Identity;
using HCM.Domain.ViewModels.Identity;
using Microsoft.AspNet.Identity;

namespace HCM.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandlerService tokenHandlerService;
        private readonly IUserHelper userHelper;

        public AccountService(
            IUserRepository userRepository,
            ITokenHandlerService tokenHandlerService,
            IUserHelper userHelper)
        {
            this.userRepository = userRepository;
            this.tokenHandlerService = tokenHandlerService;
            this.userHelper = userHelper;
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

        public async Task UpdateAccountAsync(UserModel model)
        {
            var user = await userRepository.GetAsync(model.Id)
                ?? throw new Exception(Strings.UserNotFound);

            var isUserAlreadyExists = userRepository
                .FindBy(x => x.Email == model.Email && x.Id != model.Id)
                .Any();

            if (isUserAlreadyExists)
            {
                throw new Exception(Strings.UserAlreadyExist);
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Password = PasswordHelper.HashPassword(model.Password);
            user.ModifiedById = userHelper.CurrentUserId();
            user.ModifiedOn = DateTime.UtcNow;

            await AddUserToRoleAsync(user, model.RoleId);
            await userRepository.SaveAsync();
        }

        public async Task LogoutAsync(RefreshTokenModel model)
        {
            await tokenHandlerService.RevokeRefreshTokenAsync(model);
        }

        public async Task<TokenModel> RefreshTokenAsync(RefreshTokenModel model)
        {
            return await tokenHandlerService.RefreshTokenAsync(model);
        }

        public async Task<IEnumerable<UserViewModel>> GetAllAsync()
        {
            var users = await userRepository.GetAllAsync();
            var usersViewModel = users.Select(user => new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleId = user.Roles.FirstOrDefault()?.RoleId ?? Guid.Empty,
                RoleName = user.Roles.FirstOrDefault()?.Role.Name ?? string.Empty
            });

            return usersViewModel;
        }

        public async Task<IEnumerable<RoleViewModel>> GetAllUserRolesAsync()
        {
            var roles = await userRepository.GetAllUserRolesAsync();
            var rolesViewModel = roles.Select(role => new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
            });

            return rolesViewModel;
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await userRepository.GetAsync(userId)
                ?? throw new Exception(Strings.UserNotFound);

            userRepository.Delete(user);
            await userRepository.SaveAsync();
        }

        private async Task AddUserToRoleAsync(UserEntity user, Guid roleId)
        {
            var role = await userRepository.GetRoleAsync(roleId)
                ?? throw new Exception(Strings.CannotFindUserRole);

            if (role != null)
            {
                var isUserInSameRole = await userRepository.IsUserInSameRoleAsync(user.Id, roleId);

                if (isUserInSameRole)
                {
                    throw new Exception(Strings.UserInSameRole);
                }
                else
                {
                    user.Roles.Clear();
                    user.Roles.Add(new UserRoleEntity
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    });
                }
            }
        }
    }
}
