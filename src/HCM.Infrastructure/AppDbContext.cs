using HCM.Domain.Entities;
using HCM.Domain.Entities.Identity;
using HCM.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace HCM.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        { }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<UserTokenEntity> UserTokens { get; set; }

        public DbSet<RoleEntity> Roles { get; set; }

        public DbSet<UserRoleEntity> UserRoles { get; set; }

        public DbSet<ManagerEntity> Managers { get; set; }

        public DbSet<EmployeeEntity> Employees { get; set; }

        public DbSet<AddressEntity> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfigurations());
            builder.ApplyConfiguration(new UserRoleConfigurations());
            builder.ApplyConfiguration(new ManagerConfigurations());

            base.OnModelCreating(builder);
        }
    }
}
