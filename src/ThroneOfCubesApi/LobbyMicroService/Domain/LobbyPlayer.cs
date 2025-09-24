namespace LobbyMicroService.Domain;

public class LobbyPlayer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid LobbyId { get; set; }
    public string UserId { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public bool IsOwner { get; set; }
}
