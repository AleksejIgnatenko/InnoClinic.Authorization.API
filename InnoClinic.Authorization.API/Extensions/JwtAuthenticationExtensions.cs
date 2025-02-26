using InnoClinic.Authorization.Infrastructure.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InnoClinic.Authorization.API.Extensions
{
    public static class JwtAuthenticationExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Load JWT settings
            services.Configure<JwtOptions>(configuration.GetSection("JwtSettings"));
            var jwtOptions = configuration.GetSection("JwtSettings").Get<JwtOptions>();

            // Add JWT bearer authentication
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
