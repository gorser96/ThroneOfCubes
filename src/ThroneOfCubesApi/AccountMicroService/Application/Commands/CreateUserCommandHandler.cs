using AccountMicroService.Application.Interfaces;
using AccountMicroService.Application.Queries;
using AccountMicroService.Application.Validators;
using AccountMicroService.Domain.Services;
using MediatR;

namespace AccountMicroService.Application.Commands;

public class CreateUserCommandHandler(
    IUnitOfWork unitOfWork,
    PasswordService passwordService,
    UserQueries userQueries,
    JwtTokenService jwtTokenService) : IRequestHandler<CreateUserCommand, string>
{
    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var foundUser = userQueries.FindUserByUsername(request.RegisterModel.Username);
        if (foundUser is not null)
        {
            throw new BadHttpRequestException($"Username {request.RegisterModel.Username} already taken");
        }

        UserValidator.ValidateUser(request.RegisterModel);

        var passwordHash = passwordService.GetPasswordHash(request.RegisterModel.Password);
        var user = unitOfWork.Users.Create(Guid.NewGuid(), request.RegisterModel.Username, passwordHash);

        await unitOfWork.SaveChangesAsync();

        var jwt = jwtTokenService.GenerateToken(user.Uid, request.RegisterModel.Username, user.Role);
        return jwt;
    }
}
