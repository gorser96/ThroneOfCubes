using AccountMicroService.Application.Models;
using MediatR;

namespace AccountMicroService.Domain.Entities;

public class User : IEntity
{
    private readonly List<INotification> _domainEvents = new List<INotification>();

    public User(Guid uid, string username)
    {
        Uid = uid;
        Username = username;
    }

    public Guid Uid { get; init; }
    public string Username { get; private set; }

    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents;

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public UserViewModel GetViewModel()
    {
        return new UserViewModel
        {
            Username = Username,
            Uid = Uid,
        };
    }
}
