using AccountMicroService.Domain.Entities;
using AccountMicroService.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AccountMicroService.Infrastructure.Data;

internal class AccountDbContext : DbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Uid);
            entity.Property(x => x.Uid).ValueGeneratedNever();
            entity.Property(x => x.Username).IsRequired().HasMaxLength(100);
        });
    }

    public async Task<int> SaveChangesWithEventsAsync(IMediator mediator)
    {
        var domainEntities = ChangeTracker
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

        var result = await base.SaveChangesAsync();

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
}
