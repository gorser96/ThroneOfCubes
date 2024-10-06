using AccountMicroService.Application.Commands;
using AccountMicroService.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountMicroService.API;

[ApiController]
[Route("[controller]")]
internal class UserController(
    IMediator mediator) : ControllerBase
{
    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        var jwt = await mediator.Send(new LoginUserCommand(loginModel));
        return Ok(new { jwt });
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        var jwt = await mediator.Send(new CreateUserCommand(registerModel));
        return Ok(new { jwt });
    }
}
