using AccountMicroService.Domain.Repositories;

namespace AccountMicroService.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepo Users { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
