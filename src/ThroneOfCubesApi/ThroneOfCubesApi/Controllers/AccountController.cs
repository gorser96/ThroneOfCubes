using Microsoft.AspNetCore.Mvc;
using ThroneOfCubesApi.Application.Models;
using ThroneOfCubesApi.Application.Services;

namespace ThroneOfCubesApi.Controllers;


[ApiController]
[Route("[controller]")]
public class AccountController(AccountService accountService) : ControllerBase
{
    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        var jwt = await accountService.Login(loginModel);
        return Ok(jwt);
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        var jwt = await accountService.Register(registerModel);
        return Ok(jwt);
    }
}
