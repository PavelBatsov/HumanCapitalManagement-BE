using HCM.Domain.Entities;
using HCM.Domain.Helpers;
using HCM.Domain.Interfaces;
using HCM.Domain.Interfaces.Repositories;
using HCM.Domain.Interfaces.Services;
using HCM.Domain.Localization;
using HCM.Domain.Models.Manager;
using HCM.Domain.ViewModels.Manager;

namespace HCM.Application.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository managerRepository;
        private readonly IUserHelper userHelper;

        public ManagerService(
            IManagerRepository managerRepository,
            IUserHelper userHelper)
        {
            this.managerRepository = managerRepository;
            this.userHelper = userHelper;
        }

        public async Task CreateAsync(ManagerModel model)
        {
            var isManagerAlreadyExists = managerRepository
                .FindBy(x => x.Email == model.Email)
                .Any();

            if (isManagerAlreadyExists)
            {
                throw new Exception(Strings.ManagerAlreadyExists);
            }

            var manager = new ManagerEntity
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                CreatedById = userHelper.CurrentUserId(),
                CreatedOn = DateTime.UtcNow,
                ManagerType = model.ManagerType
            };

            await managerRepository.AddAsync(manager);
            await managerRepository.SaveAsync();
        }

        public async Task<ManagerViewModel> UpdateAsync(ManagerModel model)
        {
            var manager = await managerRepository.GetAsync(model.Id)
                ?? throw new Exception(Strings.ManagerNotFound);

            var isManagerAlreadyExists = managerRepository
                .FindBy(x => x.Email == model.Email && x.Id != model.Id)
                .Any();

            if (isManagerAlreadyExists)
            {
                throw new Exception(string.Format(Strings.ManagerWithThatEmailExist, model.Email));
            }

            var dateTimeUtcNow = DateTime.UtcNow;

            manager.FirstName = model.FirstName;
            manager.LastName = model.LastName;
            manager.Email = model.Email;
            manager.PhoneNumber = model.PhoneNumber;
            manager.Address = model.Address;
            manager.CreatedOn = dateTimeUtcNow;
            manager.ModifiedById = userHelper.CurrentUserId();
            manager.ModifiedOn = dateTimeUtcNow;
            manager.ManagerType = model.ManagerType;

            await managerRepository.SaveAsync();

            return new ManagerViewModel
            {
                Id = manager.Id,
                Email = manager.Email,
                FirstName = manager.FirstName,
                LastName = manager.LastName,
                PhoneNumber = manager.PhoneNumber,
                Address = manager.Address,
                ManagerType = manager.ManagerType
            };
        }

        public async Task DeleteAsync(Guid managerId)
        {
            var manager = await managerRepository.GetAsync(managerId)
                ?? throw new Exception(Strings.ManagerNotFound);

            managerRepository.Delete(manager);
            await managerRepository.SaveAsync();
        }

        public async Task<IEnumerable<ManagerViewModel>> GetAllAsync()
        {
            var managers = await managerRepository.GetAllAsync();
            var managersViewModel = managers.Select(m => new ManagerViewModel
            {
                Id = m.Id,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Email = m.Email,
                PhoneNumber = m.PhoneNumber,
                Address = m.Address,
                ManagerType = m.ManagerType
            });

            return managersViewModel;
        }
    }
}
