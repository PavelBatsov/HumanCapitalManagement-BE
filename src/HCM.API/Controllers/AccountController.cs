using HCM.Domain.Constants;
using HCM.Domain.Interfaces.Services;
using HCM.Domain.Models.Identity;
using HCM.Domain.ViewModels.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCM.API.Controllers
{
    [ApiController]
    [Authorize(Policy = RoleConstants.Admin)]
    [Route(RoutingConstants.Controller)]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService) 
            => this.accountService = accountService;

        [AllowAnonymous]
        [HttpPost(RoutingConstants.Action)]
        public async Task<LoginViewModel> Login(LoginModel model)
           => await accountService.LoginAsync(model);

        [AllowAnonymous]
        [HttpPost(RoutingConstants.Action)]
        public async Task Register([FromBody] UserModel model)
           => await accountService.RegisterAsync(model);

        [HttpPost(RoutingConstants.Action)]
        public async Task<UserViewModel> UpdateAccount(UserModel model)
           => await accountService.UpdateAccountAsync(model);

        [AllowAnonymous]
        [HttpPost(RoutingConstants.Action)]
        public async Task<TokenModel> RefreshToken(RefreshTokenModel model)
            => await accountService.RefreshTokenAsync(model);

        [HttpPost(RoutingConstants.Action)]
        public async Task Logout(RefreshTokenModel model)
            => await accountService.LogoutAsync(model);

        [AllowAnonymous] // TEMPORARY: Remove this when the API is secured
        [HttpGet(RoutingConstants.Action)]
        public async Task<IEnumerable<UserViewModel>> GetAll()
           => await accountService.GetAllAsync();

        [AllowAnonymous] // TEMPORARY: Remove this when the API is secured
        [HttpGet(RoutingConstants.Action)]
        public async Task<IEnumerable<RoleViewModel>> GetAllUserRoles()
           => await accountService.GetAllUserRolesAsync();
    }
}