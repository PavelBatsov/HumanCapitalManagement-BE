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
        }
    }
}
