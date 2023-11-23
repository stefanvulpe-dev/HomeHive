using dotenv.net;
using HomeHive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HomeHive.Infrastructure;

public class HomeHiveContext: DbContext, IDesignTimeDbContextFactory<HomeHiveContext>
{
    public DbSet<User>? Users { get; set; }
    public DbSet<Photo>? Photos { get; set; }
    public DbSet<Contract>? Contracts { get; set; }
    public DbSet<Estate>? Estates { get; set; }
    public DbSet<Room>? Rooms { get; set; }

    public HomeHiveContext()
    {
        DotEnv.Load();
    }

    public HomeHiveContext(
        DbContextOptions<HomeHiveContext> options) :
        base(options)
    {

    }

    public HomeHiveContext CreateDbContext(string[] args)
    {
        var connectionString = DotEnv.Read()["HOME_HIVE_CONNECTION_STRING"];
        var optionsBuilder = new DbContextOptionsBuilder<HomeHiveContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return new HomeHiveContext(optionsBuilder.Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(p => p.Email).IsUnique(true);
    }
}