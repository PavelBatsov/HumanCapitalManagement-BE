using HCM.Domain.Entities;
using HCM.Domain.Interfaces.Repositories;

namespace HCM.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<UserEntity>, IUserRepository
    {
        private readonly AppDbContext dbContext;

        public UserRepository(AppDbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
