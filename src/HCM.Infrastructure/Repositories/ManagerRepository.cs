using HCM.Domain.Entities;
using HCM.Domain.Interfaces.Repositories;

namespace HCM.Infrastructure.Repositories
{
    public class ManagerRepository : GenericRepository<ManagerEntity>, IManagerRepository
    {
        public ManagerRepository(AppDbContext context)
            : base(context)
        {
            
        }
    }
}
