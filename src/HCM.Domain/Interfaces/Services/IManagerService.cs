using HCM.Domain.Models.Manager;
using HCM.Domain.ViewModels.Manager;

namespace HCM.Domain.Interfaces.Services
{
    public interface IManagerService
    {
        Task CreateAsync(ManagerModel model);

        Task UpdateAsync(ManagerModel model);

        Task DeleteAsync(Guid managerId);

        Task<IEnumerable<ManagerViewModel>> GetAllAsync();
    }
}
