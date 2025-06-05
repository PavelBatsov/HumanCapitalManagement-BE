using HCM.Domain.Entities;
using HCM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HCM.Infrastructure.Repositories
{
    public class EmployeeRepository : GenericRepository<EmployeeEntity>, IEmployeeRepository
    {
        private readonly AppDbContext context;

        public EmployeeRepository(AppDbContext context)
            : base(context)
        { 
            this.context = context;
        }

        public override async Task<IEnumerable<EmployeeEntity>> GetAllAsync()
        {
            var employees = await context.Set<EmployeeEntity>()
                .AsNoTracking()
                .ToListAsync();

            return employees;
        }
    }
}
