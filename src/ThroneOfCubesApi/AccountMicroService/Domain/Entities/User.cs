using AccountMicroService.Application.Models;
using MediatR;

namespace AccountMicroService.Domain.Entities;

public class User : IEntity
{
    private readonly List<INotification> _domainEvents = new();

    public User(Guid uid, string username, string passwordHash)
    {
        Uid = uid;
        Username = username;
        PasswordHash = passwordHash;
        Role = Roles.User;
    }

    public Guid Uid { get; init; }
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }

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
            Role = Role,
        };
    }
}
