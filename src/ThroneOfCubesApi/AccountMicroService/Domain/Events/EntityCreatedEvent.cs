using AccountMicroService.Domain.Entities;
using MediatR;

namespace AccountMicroService.Domain.Events;

public class EntityCreatedEvent : INotification
{
    public EntityCreatedEvent(IEntity entity, string entityClassName)
    {
        Entity = entity;
        EntityClassName = entityClassName;
    }

    public IEntity Entity { get; }
    public string EntityClassName { get; }
}
