using AccountMicroService.Application.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccountMicroService.Domain.Services;

public class JwtTokenService
{
    private readonly JwtOptions _jwtOptions;

    public JwtTokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(Guid userUid, string username, string role)
    {
        // Создание описания токена (claims)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userUid.ToString()),  // User ID
            new Claim(JwtRegisteredClaimNames.UniqueName, username),    // Имя пользователя
            new Claim(ClaimTypes.Role, role),                          // Роль пользователя
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())  // Уникальный идентификатор токена
        };

        // Создание ключа для подписи
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Настройка параметров токена
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes),
            signingCredentials: creds
        );

        // Генерация и возврат JWT
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
