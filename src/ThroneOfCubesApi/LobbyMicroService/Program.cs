using LobbyMicroService.Auth;
using LobbyMicroService.Endpoints;
using LobbyMicroService.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Auth"));
var jwt = builder.Configuration.GetSection("Auth").Get<JwtOptions>() ?? new JwtOptions();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    var cs = builder.Configuration.GetConnectionString("Postgres")
             ?? builder.Configuration["POSTGRES_CONNECTION"]
             ?? "Host=lobby-db;Port=5432;Database=lobbydb;Username=postgres;Password=postgres";
    opt.UseNpgsql(cs);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = jwt.Authority;                 // пример: "http://accountmicroservice:8080"
        options.Audience = jwt.Audience;                   // "lobby.api"
        options.RequireHttpsMetadata = jwt.RequireHttpsMetadata;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*
builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});
*/

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddHealthChecks()
    .AddCheck("Base", () => HealthCheckResult.Healthy("Ok"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapLobbyEndpoints();
app.MapHealthChecks("/healthz");

app.Run();
