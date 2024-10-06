using AccountMicroService.Application.Queries;
using AccountMicroService.Domain.Services;
using MediatR;

namespace AccountMicroService.Application.Commands;

public class LoginUserCommandHandler(
    UserQueries userQueries,
    PasswordService passwordService,
    JwtTokenService jwtTokenService) : IRequestHandler<LoginUserCommand, string>
{
    public Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var authUser = userQueries.FindUserByUsername(request.LoginModel.Username)
            ?? throw new UnauthorizedAccessException();

        var isVerified = passwordService.VerifyPassword(request.LoginModel.Password, authUser.PasswordHash);

        if (!isVerified)
        {
            throw new UnauthorizedAccessException();
        }

        var jwt = jwtTokenService.GenerateToken(authUser.Uid, authUser.Username, authUser.Role);

        return Task.FromResult(jwt);
    }
}
