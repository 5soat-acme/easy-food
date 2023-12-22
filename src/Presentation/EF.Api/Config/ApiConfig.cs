using System.Reflection;
using EF.Api.Extensions;
using Microsoft.OpenApi.Models;

namespace EF.Api.Config;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Easy Food", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description =
                    "Por favor insira o token JWT com 'Bearer ' prefixo. O cliente tem a opção de se autenticar ou não, porém é obrigatório o envio de um access token para identificação do carrinho.",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
        services.RegisterServices(configuration);
        services.AddIdentityConfig(configuration);

        return services;
    }

    public static WebApplication UseApiConfig(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.UseIdentityConfig();

        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}