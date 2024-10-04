using MediatR;

namespace AccountMicroService.Domain.Entities;

public interface IEntity
{
    IReadOnlyCollection<INotification> DomainEvents { get; }
    void ClearDomainEvents();
}
