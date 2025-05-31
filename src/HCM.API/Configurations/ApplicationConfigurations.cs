using HCM.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HCM.API.Configurations
{
    public static class ApplicationConfigurations
    {
        public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
        }

        public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<AppDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
