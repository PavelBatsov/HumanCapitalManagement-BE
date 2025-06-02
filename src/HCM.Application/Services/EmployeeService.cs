using HCM.Domain.Entities;
using HCM.Domain.Helpers;
using HCM.Domain.Interfaces;
using HCM.Domain.Interfaces.Repositories;
using HCM.Domain.Interfaces.Services;
using HCM.Domain.Localization;
using HCM.Domain.Models.Employee;
using HCM.Domain.ViewModels.Employee;

namespace HCM.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IUserHelper userHelper;
        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IUserHelper userHelper)
        {
            this.employeeRepository = employeeRepository;
            this.userHelper = userHelper;
        }

        public async Task CreateAsync(EmployeeModel model)
        {
            var isEmployeeAlreadyExists = employeeRepository
                .FindBy(x => x.Email == model.Email)
                .Any();

            if (isEmployeeAlreadyExists)
            {
                throw new Exception(Strings.EmployeeAlreadyExists);
            }

            var currentUserId = userHelper.CurrentUserId();
            var employee = new EmployeeEntity
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                CreatedById = currentUserId,
                CreatedOn = DateTime.UtcNow,
                EmployeeType = model.EmployeeType,
                ManagerId = currentUserId
            };

            await employeeRepository.AddAsync(employee);
            await employeeRepository.SaveAsync();
        }

        public async Task<EmployeeViewModel> UpdateAsync(EmployeeModel model)
        {
            var employee = await employeeRepository.GetAsync(model.Id)
                ?? throw new Exception(Strings.EmployeeAlreadyExists);

            var isEmployeeAlreadyExists = employeeRepository
                .FindBy(x => x.Email == model.Email && x.Id != model.Id)
                .Any();

            if (isEmployeeAlreadyExists)
            {
                throw new Exception(string.Format(Strings.EmployeeWithThatEmailExist, model.Email));
            }

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Email = model.Email;
            employee.PhoneNumber = model.PhoneNumber;
            employee.Address = model.Address;
            employee.ModifiedOn = DateTime.UtcNow;
            employee.ModifiedById = userHelper.CurrentUserId();
            employee.EmployeeType = model.EmployeeType;

            await employeeRepository.SaveAsync();

            return new EmployeeViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                EmployeeType = employee.EmployeeType
            };
        }

        public async Task DeleteAsync(Guid employeeId)
        {
            var employee = await employeeRepository.GetAsync(employeeId)
                ?? throw new Exception(Strings.EmployeeAlreadyExists);

            employeeRepository.Delete(employee);
            await employeeRepository.SaveAsync();
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAllAsync()
        {
            var employees = await employeeRepository.GetAllAsync();
            var employeeViewModel = employees.Select(e => new EmployeeViewModel
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                Address = e.Address,
                EmployeeType = e.EmployeeType
            });

            return employeeViewModel;
        }
    }
}
