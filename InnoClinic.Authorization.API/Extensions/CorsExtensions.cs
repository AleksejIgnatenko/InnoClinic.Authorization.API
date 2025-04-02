namespace InnoClinic.Authorization.API.Extensions
{
    public static class CorsExtensions
    {
        public static void AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithHeaders().AllowAnyHeader()
                           .WithOrigins("http://localhost:4000", "http://localhost:4001")
                           .WithMethods().AllowAnyMethod();
                });
            });
        }
    }
}
