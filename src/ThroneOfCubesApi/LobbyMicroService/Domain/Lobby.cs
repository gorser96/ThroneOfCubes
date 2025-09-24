namespace LobbyMicroService.Domain;

public class Lobby
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public int MaxPlayers { get; set; } = 8;
    public bool IsPrivate { get; set; }
    public string? PasswordHash { get; set; }   // если нужны приватные лобби
    public string OwnerUserId { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public bool IsStarted { get; set; }

    public List<LobbyPlayer> Players { get; set; } = new();
}
