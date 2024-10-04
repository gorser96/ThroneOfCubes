using AccountMicroService.Domain.Entities;
using MediatR;

namespace AccountMicroService.Domain.Events;

public class EntityDeletedEvent : INotification
{
    public EntityDeletedEvent(IEntity entity, string entityClassName)
    {
        Entity = entity;
        EntityClassName = entityClassName;
    }

    public IEntity Entity { get; }
    public string EntityClassName { get; }
}
