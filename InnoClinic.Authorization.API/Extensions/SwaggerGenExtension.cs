using Microsoft.OpenApi.Models;

namespace InnoClinic.Authorization.API.Extensions;

/// <summary>
/// Extension class for configuring Swagger with JWT authentication support.
/// </summary>
public static class SwaggerGenExtension
{
    /// <summary>
    /// Adds custom Swagger configuration with Bearer token support to the service collection.
    /// </summary>
    /// <param name="services">The service collection of the application.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Please insert JWT token into field",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new List<string>()
                }
            });
        });

        return services;
    }
}