using AccountMicroService.Domain.Entities;

namespace AccountMicroService.Domain.Repositories;

public interface IUserRepo
{
    User? Find(Guid uid);
    User Create(Guid uid, string username, string passwordHash);
}
