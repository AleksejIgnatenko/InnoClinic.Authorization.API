using InnoClinic.Authorization.Infrastructure.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InnoClinic.Authorization.API.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring JWT (JSON Web Token) authentication.
    /// </summary>
    public static class JwtAuthenticationExtensions
    {
        /// <summary>
        /// Adds JWT authentication services to the specified service collection.
        /// </summary>
        /// <param name="services">The service collection to which the JWT authentication will be added.</param>
        /// <param name="configuration">The configuration that contains JWT settings.</param>
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("JwtSettings"));
            var jwtOptions = configuration.GetSection("JwtSettings").Get<JwtOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudiences = new List<string> { jwtOptions.Audience },
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.SecretKey))
                    };
                });
        }
    }
}