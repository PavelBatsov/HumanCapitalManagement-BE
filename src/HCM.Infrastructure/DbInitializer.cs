using HCM.Domain.Configurations;
using HCM.Domain.Constants;
using HCM.Domain.Entities;
using HCM.Domain.Entities.Identity;
using HCM.Domain.Helpers;
using HCM.Domain.Interfaces.Repositories;
using HCM.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace HCM.Infrastructure
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider services)
        {
            using IServiceScope serviceScope = services.CreateScope();
            AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userConfiguration = serviceScope.ServiceProvider.GetRequiredService<IOptions<UserConfiguration>>().Value;
            var userRepository = serviceScope.ServiceProvider.GetRequiredService<IUserRepository>();

            context.Database.Migrate();

            InitializeUserRoles(context, userConfiguration, userRepository);
        }

        private static void InitializeUserRoles(AppDbContext context, UserConfiguration userConfiguration, IUserRepository userRepository)
        {
            // Initialize Roles
            var adminRoleId = Guid.NewGuid();
            var adminRole = context.Set<RoleEntity>().FirstOrDefault(x => x.Name == RoleConstants.Admin);

            if (adminRole == null)
            {
                adminRole = new RoleEntity()
                {
                    Id = adminRoleId,
                    Name = RoleConstants.Admin
                };

                context.Roles.Add(adminRole);
            }

            var managerRole = context.Set<RoleEntity>().FirstOrDefault(x => x.Name == RoleConstants.Manager);

            if (managerRole == null)
            {
                managerRole = new RoleEntity()
                {
                    Name = RoleConstants.Manager
                };

                context.Roles.Add(managerRole);
            }

            var employeeRole = context.Set<RoleEntity>().FirstOrDefault(x => x.Name == RoleConstants.Employee);

            if (employeeRole == null)
            {
                employeeRole = new RoleEntity()
                {
                    Name = RoleConstants.Employee
                };

                context.Roles.Add(employeeRole);
            }

            // Initialize Users
            var userId = Guid.NewGuid();
            var email = userConfiguration.Email;
            var user = userRepository.GetUserByEmailAsync(email).Result;

            if (user == null)
            {
                user = new UserEntity
                {
                    Id = userId,
                    UserName = email,
                    Email = email,
                    FirstName = userConfiguration.FirstName,
                    LastName = userConfiguration.LastName,
                    Password = PasswordHelper.HashPassword(userConfiguration.Password),
                    Roles =
                    [
                        new UserRoleEntity
                        {
                            UserId = userId,
                            RoleId = adminRoleId
                        }
                    ],
                    CreatedById = userId,
                    CreatedOn = DateTime.UtcNow,
                };

                userRepository.Add(user);
            }

            context.SaveChanges();
        }
    }
}
