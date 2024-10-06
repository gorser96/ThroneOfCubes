using AccountMicroService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountMicroService.Infrastructure.Data;

public class AccountDbContext : DbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Uid);
            entity.Property(x => x.Uid).ValueGeneratedNever();
            entity.Property(x => x.Username).IsRequired().HasMaxLength(100);
        });
    }
}
