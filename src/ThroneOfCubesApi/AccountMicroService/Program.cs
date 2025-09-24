using AccountMicroService.Application.Interfaces;
using AccountMicroService.Application.Models;
using AccountMicroService.Application.Queries;
using AccountMicroService.Domain.Repositories;
using AccountMicroService.Domain.Services;
using AccountMicroService.Infrastructure.Data;
using AccountMicroService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddDbContext<AccountDbContext>(opt =>
{
    var cs = builder.Configuration.GetConnectionString("Postgres")
             ?? builder.Configuration["POSTGRES_CONNECTION"]
             ?? "Host=users-db;Port=5432;Database=lobbydb;Username=postgres;Password=postgres";
    opt.UseNpgsql(cs);
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddScoped<UserQueries>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<JwtTokenValidationService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepo, UserRepo>();

builder.Services.AddHealthChecks()
    .AddCheck("Base", () => HealthCheckResult.Healthy("Ok"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
    await db.Database.MigrateAsync();
}

//app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/healthz");

app.Run();
