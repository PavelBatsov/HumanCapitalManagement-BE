using HCM.Domain.Constants;
using HCM.Domain.Interfaces.Services;
using HCM.Domain.Models.Manager;
using HCM.Domain.ViewModels.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCM.API.Controllers
{
    [ApiController]
    [Authorize(Policy = RoleConstants.Manager)]
    [Route(RoutingConstants.Controller)]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService managerService;

        public ManagerController(IManagerService managerService)
            => this.managerService = managerService;

        [Authorize(Policy = RoleConstants.Admin)]
        [HttpPost(RoutingConstants.Action)]
        public async Task Create([FromForm] ManagerModel model)
            => await managerService.CreateAsync(model);

        [Authorize(Policy = RoleConstants.Admin)]
        [HttpPut(RoutingConstants.Action)]
        public async Task Update([FromForm] ManagerModel model)
            => await managerService.UpdateAsync(model);

        [Authorize(Policy = RoleConstants.Admin)]
        [HttpDelete(RoutingConstants.ActionId)]
        public async Task Delete([FromRoute] Guid id)
            => await managerService.DeleteAsync(id);

        [HttpGet(RoutingConstants.Action)]
        public async Task<IEnumerable<ManagerViewModel>> GetAll()
            => await managerService.GetAllAsync();
    }
}
