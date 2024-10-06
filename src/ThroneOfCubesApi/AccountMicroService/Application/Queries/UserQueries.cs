using AccountMicroService.Application.Models;
using AccountMicroService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountMicroService.Application.Queries;

public class UserQueries(AccountDbContext context)
{
    private readonly AccountDbContext _context = context;

    public UserViewModel? FindUser(Guid uid)
    {
        var obj = _context.Users.AsNoTracking().FirstOrDefault(x => x.Uid == uid);
        return obj?.GetViewModel();
    }

    public UserAuthModel? FindUserByUsername(string username)
    {
        var obj = _context.Users.AsNoTracking().FirstOrDefault(x => x.Username == username);
        if (obj is null)
        {
            return null;
        }

        return new UserAuthModel
        {
            Uid = obj.Uid,
            PasswordHash = obj.PasswordHash,
            Role = obj.Role,
        };
    }
}
