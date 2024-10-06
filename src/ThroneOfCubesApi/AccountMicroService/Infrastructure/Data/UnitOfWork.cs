using AccountMicroService.Application.Interfaces;
using AccountMicroService.Domain.Entities;
using AccountMicroService.Domain.Events;
using AccountMicroService.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AccountMicroService.Infrastructure.Data;

public class UnitOfWork(
    AccountDbContext context, 
    IMediator mediator, 
    IUserRepo userRepository) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    public IUserRepo Users { get; } = userRepository;

    public async Task BeginTransactionAsync()
    {
        _transaction = await context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction is null)
        {
            throw new ApplicationException("Transaction not openned!");
        }

        try
        {
            await context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction is null)
        {
            return;
        }

        await _transaction.RollbackAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        var domainEntities = context.ChangeTracker
            .Entries<IEntity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        foreach (var entry in domainEntities)
        {
            var entity = entry.Entity;
            var state = entry.State;

            if (state == EntityState.Modified)
            {
                var updateEvent = new EntityUpdatedEvent(entity, entity.GetType().Name);
                domainEvents.Add(updateEvent);
            }
            else if (state == EntityState.Deleted)
            {
                var deleteEvent = new EntityDeletedEvent(entity, entity.GetType().Name);
                domainEvents.Add(deleteEvent);
            }
            else if (state == EntityState.Added)
            {
                var createdEvent = new EntityCreatedEvent(entity, entity.GetType().Name);
                domainEvents.Add(createdEvent);
            }
        }

        var result = await SaveChangesAsync();

        foreach (var entity in domainEntities)
        {
            entity.Entity.ClearDomainEvents();
        }

        foreach (var item in domainEvents)
        {
            await mediator.Publish(item);
        }

        return result;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        context.Dispose();
    }
}