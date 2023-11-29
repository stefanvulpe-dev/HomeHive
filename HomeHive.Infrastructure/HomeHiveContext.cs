using HomeHive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeHive.Infrastructure;

public class HomeHiveContext : DbContext
{
    public HomeHiveContext(
        DbContextOptions<HomeHiveContext> options) :
        base(options)
    {
    }

    public DbSet<Photo>? Photos { get; set; }
    public DbSet<Contract>? Contracts { get; set; }
    public DbSet<Estate>? Estates { get; set; }
    public DbSet<Room>? Rooms { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Estate>().Ignore(e => e.Owner);
        builder.Entity<Contract>().Ignore(c => c.User);
    }
}