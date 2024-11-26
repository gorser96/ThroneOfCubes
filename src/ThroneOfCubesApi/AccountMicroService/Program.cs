using AccountMicroService.Application.Interfaces;
using AccountMicroService.Application.Models;
using AccountMicroService.Application.Queries;
using AccountMicroService.Domain.Repositories;
using AccountMicroService.Domain.Services;
using AccountMicroService.Infrastructure.Data;
using AccountMicroService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddDbContext<AccountDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString")));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddScoped<UserQueries>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<JwtTokenValidationService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepo, UserRepo>();

var app = builder.Build();

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
