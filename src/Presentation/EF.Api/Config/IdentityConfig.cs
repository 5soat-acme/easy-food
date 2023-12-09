using EF.Identidade.Infra.Data;
using EF.Identidade.Infra.Extensions;
using EF.WebApi.Commons.Identity;
using EF.WebApi.Commons.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EF.Api.Config;

public static class IdentityConfig
{
    public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUserApp, UserApp>();

        services.AddDbContext<IdentidadeDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddDefaultIdentity<IdentityUser>()
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