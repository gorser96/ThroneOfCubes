using AccountMicroService.Application.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccountMicroService.Domain.Services;

public class JwtTokenValidationService
{
    private readonly JwtOptions _jwtOptions;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly ILogger<JwtTokenValidationService> _logger;

    public JwtTokenValidationService(
        IOptions<JwtOptions> jwtOptions,
        ILogger<JwtTokenValidationService> logger)
    {
        _jwtOptions = jwtOptions.Value;
        _logger = logger;

        _tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
            ClockSkew = TimeSpan.Zero // Убираем задержку для более точной проверки срока действия токена
        };
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

            // Проверяем, что алгоритм токена совпадает с ожидаемым (HMACSHA256)
            if (validatedToken is JwtSecurityToken jwtToken &&
                jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return principal;
            }
            else
            {
                throw new SecurityTokenException("Invalid token");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Token validation failed: {msg}", ex.Message);
            return null;
        }
    }
}
