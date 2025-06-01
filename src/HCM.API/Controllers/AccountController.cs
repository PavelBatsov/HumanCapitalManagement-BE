using HCM.Domain.Constants;
using HCM.Domain.Interfaces.Services;
using HCM.Domain.Models.Identity;
using HCM.Domain.ViewModels.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCM.API.Controllers
{
    [ApiController]
    [Route(RoutingConstants.Controller)]
    public class AccountController
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost(RoutingConstants.Action)]
        public async Task<LoginViewModel> Login(LoginModel model)
           => await accountService.LoginAsync(model);

        [AllowAnonymous]
        [HttpPost(RoutingConstants.Action)]
        public async Task Register(UserModel model)
           => await accountService.RegisterAsync(model);

        [Authorize(Policy = RoleConstants.Admin)]
        [HttpPost(RoutingConstants.Action)]
        public async Task<UserViewModel> UpdateAccount(UserModel model)
           => await accountService.UpdateAccountAsync(model);

        [Authorize(Policy = RoleConstants.Employee)]
        [HttpPost(RoutingConstants.Action)]
        public async Task<TokenModel> RefreshToken(RefreshTokenModel model)
            => await accountService.RefreshTokenAsync(model);

        [Authorize(Policy = RoleConstants.Employee)]
        [HttpPost(RoutingConstants.Action)]
        public async Task Logout(RefreshTokenModel model)
            => await accountService.LogoutAsync(model);
    }
}