using HCM.Application.Services;
using HCM.Domain.Configurations;
using HCM.Domain.Interfaces.Repositories;
using HCM.Domain.Interfaces.Services;
using HCM.Infrastructure;
using HCM.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HCM.API.Configurations
{
    public static class ApplicationConfigurations
    {
        public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            Configurations(services, configuration);
            AddServices(services);
        }

        public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<AppDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        private static void Configurations(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationConfiguration>(configuration.GetSection(nameof(AuthenticationConfiguration)));
        }

        public static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenHandlerService, TokenHandlerService>();
        }
    }
}
