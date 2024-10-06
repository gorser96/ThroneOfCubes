using AccountMicroService.Application.Models;
using MediatR;

namespace AccountMicroService.Application.Commands;

public class CreateUserCommand : IRequest<string>
{
    public CreateUserCommand(RegisterModel registerModel)
    {
        RegisterModel = registerModel;
    }

    public RegisterModel RegisterModel { get; }
}
