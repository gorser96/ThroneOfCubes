using AccountMicroService.Application.Models;
using AccountMicroService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountMicroService.Application.Queries;

internal class UserQueries(AccountDbContext context)
{
    private readonly AccountDbContext _context = context;

    public UserViewModel? FindUser(Guid uid)
    {
        var obj = _context.Users.AsNoTracking().FirstOrDefault(x => x.Uid == uid);
        return obj?.GetViewModel();
    }
}
