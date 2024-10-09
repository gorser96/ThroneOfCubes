using AccountMicroService.Application.Commands;
using AccountMicroService.Application.Models;
using AccountMicroService.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AccountMicroService.API;

[ApiController]
[Route("[controller]")]
internal class AccountController(
    IMediator mediator, JwtTokenValidationService jwtTokenValidationService) : ControllerBase
{
    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        var jwt = await mediator.Send(new LoginUserCommand(loginModel));
        return Ok(new JwtResponse { Token = jwt });
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        var jwt = await mediator.Send(new CreateUserCommand(registerModel));
        return Ok(new JwtResponse { Token = jwt });
    }

    [HttpGet("validate-token")]
    public IActionResult ValidateToken([FromHeader] string authorization)
    {
        if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
        {
            return Unauthorized("Missing or invalid token");
        }

        var token = authorization.Substring("Bearer ".Length).Trim();
        var principal = jwtTokenValidationService.ValidateToken(token);

        if (principal == null)
        {
            return Unauthorized("Invalid token");
        }

        var username = principal.FindFirstValue(JwtRegisteredClaimNames.UniqueName);
        return Ok(username);
    }
}
