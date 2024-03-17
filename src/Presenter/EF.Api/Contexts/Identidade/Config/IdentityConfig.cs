using EF.Identidade.Infra.Data;
using EF.Identidade.Infra.Extensions;
using EF.WebApi.Commons.Identity;
using EF.WebApi.Commons.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Contexts.Identidade.Config;

public static class IdentityConfig
{
    public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUserApp, UserApp>();

        services.AddDbContext<IdentidadeDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddRoles<IdentityRole>()
            .AddErrorDescriber<TraducaoPortugues>()
            .AddEntityFrameworkStores<IdentidadeDbContext>()
            .AddDefaultTokenProviders();

        services.AddJwtConfiguration(configuration);

        return services;
    }

    public static WebApplication UseIdentityConfig(this WebApplication app)
    {
        app.UseAuthConfiguration();
        return app;
    }
}