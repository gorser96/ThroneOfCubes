using AccountMicroService.Application.Models;
using AccountMicroService.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AccountMicroService.API;

[ApiController]
[Route("[controller]")]
internal class UserController(UserQueries userQueries) : ControllerBase
{
    [HttpGet("{userUid:guid}")]
    public UserViewModel? GetUser(Guid userUid)
    {
        return userQueries.FindUser(userUid);
    }
}
