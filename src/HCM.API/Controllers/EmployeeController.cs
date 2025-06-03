using HCM.Domain.Constants;
using HCM.Domain.Interfaces.Services;
using HCM.Domain.Models.Employee;
using HCM.Domain.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCM.API.Controllers
{
    [ApiController]
    [Authorize(Policy = RoleConstants.Employee)]
    [Route(RoutingConstants.Controller)]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
            => this.employeeService = employeeService;

        [Authorize(Policy = RoleConstants.Manager)]
        [HttpPost(RoutingConstants.Action)]
        public async Task Create([FromForm] EmployeeModel model)
            => await employeeService.CreateAsync(model);

        [Authorize(Policy = RoleConstants.Manager)]
        [HttpPut(RoutingConstants.Action)]
        public async Task<EmployeeViewModel> Update([FromForm] EmployeeModel model)
            => await employeeService.UpdateAsync(model);

        [Authorize(Policy = RoleConstants.Manager)]
        [HttpDelete(RoutingConstants.ActionId)]
        public async Task Delete([FromRoute] Guid id)
            => await employeeService.DeleteAsync(id);

        [HttpGet(RoutingConstants.Action)]
        public async Task<IEnumerable<EmployeeViewModel>> GetAll()
            => await employeeService.GetAllAsync();
    }
}
