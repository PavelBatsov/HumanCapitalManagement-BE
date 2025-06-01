using HCM.Application.Services;
using HCM.Domain.Configurations;
using HCM.Domain.Constants;
using HCM.Domain.Interfaces.Repositories;
using HCM.Domain.Interfaces.Services;
using HCM.Infrastructure;
using HCM.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace HCM.API.Configurations
{
    public static class ApplicationConfigurations
    {
        public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            Configurations(services, configuration);
            ConfigureAuthentication(services, configuration);
            ConfigureAuthorization(services);
            RegisterServices(services);
            RegisterRepositories(services);
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

        private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var authenticationConfiguration = configuration
                .GetSection(nameof(AuthenticationConfiguration))
                .Get<AuthenticationConfiguration>();

            var key = Encoding.UTF8.GetBytes(authenticationConfiguration.Secret);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authenticationConfiguration.Issuer,
                        ValidateAudience = true,
                        ValidAudience = authenticationConfiguration.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                });
        }

        private static void ConfigureAuthorization(IServiceCollection services)
        {
            services.Configure<AuthorizationOptions>(options =>
            {
                options.AddPolicy(RoleConstants.Admin, policy =>
                    policy.RequireClaim(ClaimTypes.Role, RoleConstants.Admin));

                options.AddPolicy(RoleConstants.Manager, policy =>
                    policy.RequireClaim(ClaimTypes.Role, RoleConstants.Admin, RoleConstants.Manager));

                options.AddPolicy(RoleConstants.Employee, policy =>
                    policy.RequireClaim(ClaimTypes.Role, RoleConstants.Admin, RoleConstants.Manager, RoleConstants.Employee));
            });
        }

        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenHandlerService, TokenHandlerService>();
        }

        public static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
