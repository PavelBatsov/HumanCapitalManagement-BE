using HCM.Domain.Constants;
using HCM.Domain.Interfaces.Services;
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
        public async Task Login()
           => await accountService.Login();
    }
}
