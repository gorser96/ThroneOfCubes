using AccountMicroService.Domain.Entities;
using AccountMicroService.Domain.Repositories;
using AccountMicroService.Infrastructure.Data;

namespace AccountMicroService.Infrastructure.Repositories;

internal class UserRepo(AccountDbContext dbContext) : IUserRepo
{
    public User Create(Guid uid, string username)
    {
        return dbContext.Users.Add(new(uid, username)).Entity;
    }

    public User? Find(Guid uid)
    {
        return dbContext.Users.Find(uid);
    }
}
