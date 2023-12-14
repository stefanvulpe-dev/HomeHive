using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Estates;
using HomeHive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HomeHive.Infrastructure;

public class HomeHiveContext(
    DbContextOptions<HomeHiveContext> options,
    ICurrentUserService currentUserService)
    : DbContext(options)
{
    public DbSet<Photo>? Photos { get; set; }
    public DbSet<Contract>? Contracts { get; set; }
    public DbSet<Estate>? Estates { get; set; }
    public DbSet<Room>? Rooms { get; set; }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = currentUserService.GetCurrentUserName();
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = currentUserService.GetCurrentUserName();
                    entry.Entity.LastModifiedDate = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = currentUserService.GetCurrentUserName();
                    entry.Entity.LastModifiedDate = DateTime.UtcNow;
                    break;
            }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var valueComparer = new ValueComparer<List<Utility>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());
        
        modelBuilder
            .Entity<Estate>()
            .Property(e => e.Utilities)
            .HasConversion(
                v => string.Join(",", v.Select(e => e.ToString("D")).ToArray()),
                v => v.Split(new[] { ',' })
                    .Select(e =>  Enum.Parse(typeof(Utility), e))
                    .Cast<Utility>()
                    .ToList()
            );
        modelBuilder
            .Entity<Estate>()
            .Property(e => e.Utilities)
            .Metadata
            .SetValueComparer(valueComparer);
        
    }
}