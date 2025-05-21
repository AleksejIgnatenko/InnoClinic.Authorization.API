using InnoClinic.Authorization.API.Extensions;
using InnoClinic.Authorization.API.Middlewares;
using InnoClinic.Authorization.Application.MapperProfiles;
using InnoClinic.Authorization.Application.RabbitMQ;
using InnoClinic.Authorization.Application.Services;
using InnoClinic.Authorization.Core.Abstractions;
using InnoClinic.Authorization.DataAccess.Context;
using InnoClinic.Authorization.DataAccess.Repositories;
using InnoClinic.Authorization.Infrastructure.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .CreateSerilog();

builder.Host.UseSerilog(Log.Logger);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCustomSwagger();

builder.Services.AddDbContext<InnoClinicAuthorizationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<RabbitMQSetting>(
    builder.Configuration.GetSection("RabbitMQ"));

// Load JWT settings
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddDataProtection();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddHostedService<RabbitMQListener>();

builder.Services.AddAutoMapper(typeof(AccountMappingProfile));

builder.Services.AddAuthentication();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var rabbitMQService = services.GetRequiredService<IRabbitMQService>();
    await rabbitMQService.CreateQueuesAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:4000", "http://localhost:4001");
    x.WithMethods().AllowAnyMethod();
    x.AllowCredentials();
});

app.Run();