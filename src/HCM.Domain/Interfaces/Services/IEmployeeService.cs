using HCM.Domain.Models.Employee;
using HCM.Domain.ViewModels.Employee;

namespace HCM.Domain.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task CreateAsync(EmployeeModel model);

        Task<EmployeeViewModel> UpdateAsync(EmployeeModel model);

        Task DeleteAsync(Guid employeeId);

        Task<IEnumerable<EmployeeViewModel>> GetAllAsync();
    }
}
