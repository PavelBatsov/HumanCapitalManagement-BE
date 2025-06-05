using HCM.Domain.Entities;
using HCM.Domain.Entities.Identity;
using HCM.Domain.Helpers;
using HCM.Domain.Interfaces;
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

            var token = await tokenHandlerService.GenerateTokenAsync(user);

            return new LoginViewModel
            {
                User = EntityToViewModel(user),
                Token = token
            };
        }

        public async Task RegisterAsync(UserModel model)
        {
            var isUserExists = await userRepository.IsUserExistsAsync(model.Email);

            if (isUserExists)
            {
                throw new Exception(Strings.UserAlreadyExist);
            }

            var user = ModelToEntityCreate(model);

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

            ModelToEntityUpdate(user, model);
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
            return await EntityToUserViewModelAsync();
        }

        public async Task<IEnumerable<RoleViewModel>> GetAllUserRolesAsync()
        {
            return await EntityToRoleViewModelAsync();
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await userRepository.GetAsync(userId)
                ?? throw new Exception(Strings.UserNotFound);

            userRepository.Delete(user);
            await userRepository.SaveAsync();
        }

        #region Private Methods

        private async Task AddUserToRoleAsync(UserEntity user, Guid roleId)
        {
            var role = await userRepository.GetRoleAsync(roleId)
                ?? throw new Exception(Strings.CannotFindUserRole);

            if (role != null)
            {
                var isUserInSameRole = await userRepository.IsUserInSameRoleAsync(user.Id, roleId);

                if (!isUserInSameRole)
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

        private static UserViewModel EntityToViewModel(UserEntity user)
        {
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleName = user.Roles.FirstOrDefault()?.Role.Name ?? string.Empty
            };

            return userViewModel;
        }

        private async Task<IEnumerable<UserViewModel>> EntityToUserViewModelAsync()
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
                RoleName = user.Roles.FirstOrDefault()?.Role.Name ?? string.Empty,
                Address = user.Address != null ? new AddressModel
                {
                    Address = user.Address.Address ?? string.Empty,
                    City = user.Address.City ?? string.Empty,
                    Country = user.Address.Country ?? string.Empty,
                    PostCode = user.Address.PostCode ?? string.Empty
                } : new AddressModel()
            });

            return usersViewModel;
        }

        private static UserEntity ModelToEntityCreate(UserModel model)
        {
            var userId = Guid.NewGuid();
            var dateTimeUtcNow = DateTime.UtcNow;
            var user = new UserEntity
            {
                Id = userId,
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = PasswordHelper.HashPassword(model.Password),
                Address = model.Address != null ? new AddressEntity
                {
                    Address = model.Address.Address ?? string.Empty,
                    City = model.Address.City ?? string.Empty,
                    Country = model.Address.Country ?? string.Empty,
                    PostCode = model.Address.PostCode ?? string.Empty,
                    CreatedOn = dateTimeUtcNow,
                    CreatedById = userId
                } : new AddressEntity(),
                Roles =
                [
                    new UserRoleEntity
                    {
                        UserId = userId,
                        RoleId = model.RoleId
                    }
                ],
                CreatedById = userId,
                CreatedOn = dateTimeUtcNow
            };

            return user;
        }

        private void ModelToEntityUpdate(UserEntity user, UserModel model)
        {
            var currentUserId = userHelper.CurrentUserId();
            var dateTimeUtcNow = DateTime.UtcNow;

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Password = PasswordHelper.HashPassword(model.Password);
            user.ModifiedById = currentUserId;
            user.ModifiedOn = dateTimeUtcNow;

            if (user.Address != null)
            {
                user.Address.Address = model.Address.Address;
                user.Address.City = model.Address.City;
                user.Address.Country = model.Address.Country;
                user.Address.PostCode = model.Address.PostCode;
                user.Address.ModifiedOn = dateTimeUtcNow;
                user.Address.ModifiedById = currentUserId;
            }
        }

        private async Task<IEnumerable<RoleViewModel>> EntityToRoleViewModelAsync()
        {
            var roles = await userRepository.GetAllUserRolesAsync();
            var rolesViewModel = roles.Select(role => new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
            });

            return rolesViewModel;
        }

        #endregion Private Methods
    }
}
