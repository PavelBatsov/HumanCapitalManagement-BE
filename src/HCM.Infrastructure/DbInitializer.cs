using HCM.Domain.Constants;
using HCM.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HCM.Infrastructure
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider services)
        {
            using IServiceScope serviceScope = services.CreateScope();
            AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.Migrate();

            InitializeRoles(context);
        }

        private static void InitializeRoles(AppDbContext context)
        {
            var adminRole = context.Set<RoleEntity>().FirstOrDefault(x => x.Name == RoleConstants.Admin);

            if (adminRole == null)
            {
                adminRole = new RoleEntity()
                {
                    Name = RoleConstants.Admin,
                };

                context.Roles.Add(adminRole);
            }

            var managerRole = context.Set<RoleEntity>().FirstOrDefault(x => x.Name == RoleConstants.Manager);

            if (managerRole == null)
            {
                managerRole = new RoleEntity()
                {
                    Name = RoleConstants.Manager,
                };

                context.Roles.Add(managerRole);
            }

            var employeeRole = context.Set<RoleEntity>().FirstOrDefault(x => x.Name == RoleConstants.Employee);

            if (employeeRole == null)
            {
                employeeRole = new RoleEntity()
                {
                    Name = RoleConstants.Employee,
                };

                context.Roles.Add(employeeRole);
            }

            context.SaveChanges();
        }
    }
}
