using HCM.Domain.Entities;
using HCM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HCM.Infrastructure.Repositories
{
    public class ManagerRepository : GenericRepository<ManagerEntity>, IManagerRepository
    {
        private readonly AppDbContext context;

        public ManagerRepository(AppDbContext context)
            : base(context)
        {
            this.context = context;
        }

        public override async Task<IEnumerable<ManagerEntity>> GetAllAsync()
        {
            var managers = await context.Set<ManagerEntity>()
                .AsNoTracking()
                .ToListAsync();

            return managers;
        }
    }
}
