namespace LobbyMicroService.DTOs;

public record CreateLobbyRequest(string Name, int MaxPlayers = 8, bool IsPrivate = false, string? Password = null);
public record LobbyResponse(Guid Id, string Name, int MaxPlayers, bool IsPrivate, bool IsStarted, int PlayerCount, string OwnerUserId, DateTimeOffset CreatedAt);
public record JoinLobbyRequest(string? Password = null, string? DisplayName = null);
public record PagedQuery(int Page = 1, int PageSize = 20);
