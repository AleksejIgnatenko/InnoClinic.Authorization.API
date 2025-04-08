using Microsoft.Extensions.DependencyInjection;

namespace InnoClinic.Authorization.API.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring CORS (Cross-Origin Resource Sharing) policies.
    /// </summary>
    public static class CorsExtensions
    {
        /// <summary>
        /// Adds a custom CORS policy to the service collection.
        /// </summary>
        /// <param name="services">The service collection to which the CORS policy will be added.</param>
        public static void AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithHeaders().AllowAnyHeader()
                           .WithOrigins("http://localhost:4000", "http://localhost:4001", "http://localhost:5005")
                           .WithMethods().AllowAnyMethod();
                });
            });
        }
    }
}