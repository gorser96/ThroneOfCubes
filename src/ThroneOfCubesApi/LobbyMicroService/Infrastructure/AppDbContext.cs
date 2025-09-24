using LobbyMicroService.Domain;
using Microsoft.EntityFrameworkCore;

namespace LobbyMicroService.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Lobby> Lobbies => Set<Lobby>();
    public DbSet<LobbyPlayer> Players => Set<LobbyPlayer>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Lobby>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.HasMany(x => x.Players).WithOne().HasForeignKey(p => p.LobbyId).OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => x.IsStarted);
            e.HasIndex(x => x.CreatedAt);
        });

        b.Entity<LobbyPlayer>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.LobbyId, x.UserId }).IsUnique();
            e.Property(x => x.DisplayName).HasMaxLength(50).IsRequired();
        });
    }
}
