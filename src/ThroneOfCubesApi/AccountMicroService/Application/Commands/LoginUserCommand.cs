using AccountMicroService.Application.Models;
using MediatR;

namespace AccountMicroService.Application.Commands;

public class LoginUserCommand : IRequest<string>
{
    public LoginUserCommand(LoginModel loginModel)
    {
        LoginModel = loginModel;
    }

    public LoginModel LoginModel { get; }
}
