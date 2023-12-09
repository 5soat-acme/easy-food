using System.Text;
using EF.Identidade.Infra.Data;
using EF.Identidade.Infra.Extensions;
using EF.WebApi.Commons.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EF.Api.Config;

public static class IdentityConfig
{
    public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAppUser, AppUser>();

        services.AddDbContext<IdentidadeDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddErrorDescriber<TraducaoPortugues>()
            .AddEntityFrameworkStores<IdentidadeDbContext>()
            .AddDefaultTokenProviders();

        // JWT

        var settingsSection = configuration.GetSection("IdentidadeSettings");
        services.Configure<IdentitySettings>(settingsSection);
        var identitySettings = settingsSection.Get<IdentitySettings>();
        var key = Encoding.ASCII.GetBytes(identitySettings.Secret);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearerOptions =>
        {
            bearerOptions.RequireHttpsMetadata = true;
            bearerOptions.SaveToken = true;
            bearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = identitySettings.ValidIn,
                ValidIssuer = identitySettings.Issuer
            };
        });

        return services;
    }

    public static WebApplication UseIdentityConfig(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}