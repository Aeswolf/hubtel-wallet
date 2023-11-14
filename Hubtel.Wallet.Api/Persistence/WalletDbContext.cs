using Hubtel.Wallet.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Wallet.Api.Persistence;

public class WalletDbContext : DbContext
{
    public virtual DbSet<UserModel> Users { get; set; }

    public virtual DbSet<WalletModel> Wallets { get; set; }

    public WalletDbContext(DbContextOptions<WalletDbContext> dbContextOptions) : base(dbContextOptions) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WalletDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseModel>())
        {

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
