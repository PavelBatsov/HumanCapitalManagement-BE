using HCM.Domain.Entities;
using HCM.Domain.Interfaces.Repositories;

namespace HCM.Infrastructure.Repositories
{
    public class EmployeeRepository : GenericRepository<EmployeeEntity>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context)
            : base(context)
        {
        }
    }
}
