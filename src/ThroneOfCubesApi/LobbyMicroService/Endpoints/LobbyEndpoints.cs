using LobbyMicroService.Domain;
using LobbyMicroService.DTOs;
using LobbyMicroService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LobbyMicroService.Endpoints;

public static class LobbyEndpoints
{
    public static IEndpointRouteBuilder MapLobbyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/lobbies")
                       .RequireAuthorization();

        group.MapGet("", async (AppDbContext db, int page = 1, int pageSize = 20, bool? started = null) =>
        {
            var q = db.Lobbies.AsNoTracking().Include(l => l.Players).OrderByDescending(l => l.CreatedAt).AsQueryable();
            if (started is not null) q = q.Where(l => l.IsStarted == started.Value);
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return items.Select(ToDto);
        })
        .WithName("ListLobbies");

        group.MapGet("{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var lobby = await db.Lobbies.Include(x => x.Players).FirstOrDefaultAsync(x => x.Id == id);
            return lobby is null ? Results.NotFound() : Results.Ok(ToDto(lobby));
        })
        .WithName("GetLobby");

        group.MapPost("", async (CreateLobbyRequest req, AppDbContext db, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub")!;
            var display = user.Identity?.Name ?? user.FindFirstValue("preferred_username") ?? "Player";

            var lobby = new Lobby
            {
                Name = string.IsNullOrWhiteSpace(req.Name) ? $"Lobby-{Guid.NewGuid():N}[..6]" : req.Name,
                MaxPlayers = Math.Clamp(req.MaxPlayers, 2, 16),
                IsPrivate = req.IsPrivate,
                OwnerUserId = userId,
                // PasswordHash = req.IsPrivate && req.Password is not null ? BCrypt.Net.BCrypt.HashPassword(req.Password) : null
            };
            lobby.Players.Add(new LobbyPlayer { LobbyId = lobby.Id, UserId = userId, DisplayName = display, IsOwner = true });

            db.Lobbies.Add(lobby);
            await db.SaveChangesAsync();
            return Results.Created($"/api/lobbies/{lobby.Id}", ToDto(lobby));
        })
        .WithName("CreateLobby");

        group.MapPost("{id:guid}/join", async (Guid id, JoinLobbyRequest req, AppDbContext db, ClaimsPrincipal user) =>
        {
            var lobby = await db.Lobbies.Include(x => x.Players).FirstOrDefaultAsync(x => x.Id == id);
            if (lobby is null) return Results.NotFound();

            if (lobby.IsStarted) return Results.BadRequest(new { message = "Game already started." });
            if (lobby.Players.Count >= lobby.MaxPlayers) return Results.BadRequest(new { message = "Lobby is full." });

            // if (lobby.IsPrivate && !BCrypt.Net.BCrypt.Verify(req.Password ?? "", lobby.PasswordHash)) return Results.Forbid();

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub")!;
            if (lobby.Players.Any(p => p.UserId == userId)) return Results.NoContent();

            var display = req.DisplayName ?? user.Identity?.Name ?? "Player";
            lobby.Players.Add(new LobbyPlayer { LobbyId = lobby.Id, UserId = userId, DisplayName = display, IsOwner = false });

            await db.SaveChangesAsync();
            return Results.Ok(ToDto(lobby));
        })
        .WithName("JoinLobby");

        group.MapPost("{id:guid}/leave", async (Guid id, AppDbContext db, ClaimsPrincipal user) =>
        {
            var lobby = await db.Lobbies.Include(x => x.Players).FirstOrDefaultAsync(x => x.Id == id);
            if (lobby is null) return Results.NotFound();

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub")!;
            var player = lobby.Players.FirstOrDefault(p => p.UserId == userId);
            if (player is null) return Results.NoContent();

            db.Remove(player);

            // если владелец вышел — назначим нового владельца
            if (player.IsOwner && lobby.Players.Count > 1)
            {
                var newOwner = lobby.Players.FirstOrDefault(p => p.UserId != userId);
                if (newOwner is not null) newOwner.IsOwner = true;
            }

            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("LeaveLobby");

        group.MapPost("{id:guid}/start", async (Guid id, AppDbContext db, ClaimsPrincipal user) =>
        {
            var lobby = await db.Lobbies.Include(x => x.Players).FirstOrDefaultAsync(x => x.Id == id);
            if (lobby is null) return Results.NotFound();

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub")!;
            var me = lobby.Players.FirstOrDefault(p => p.UserId == userId);
            if (me is null || !me.IsOwner) return Results.Forbid();

            if (lobby.Players.Count < 2) return Results.BadRequest(new { message = "Need at least 2 players." });

            lobby.IsStarted = true;
            await db.SaveChangesAsync();

            // TODO: шлём событие в MatchService через шину/HTTP, создаём матч
            return Results.Ok(ToDto(lobby));
        })
        .WithName("StartLobby");

        group.MapDelete("{id:guid}", async (Guid id, AppDbContext db, ClaimsPrincipal user) =>
        {
            var lobby = await db.Lobbies.Include(x => x.Players).FirstOrDefaultAsync(x => x.Id == id);
            if (lobby is null) return Results.NotFound();

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub")!;
            if (lobby.OwnerUserId != userId) return Results.Forbid();

            db.Remove(lobby);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("DeleteLobby");
        
        group.WithOpenApi();

        return app;
    }

    private static LobbyResponse ToDto(Lobby l) =>
        new(l.Id, l.Name, l.MaxPlayers, l.IsPrivate, l.IsStarted, l.Players.Count, l.OwnerUserId, l.CreatedAt);
}
