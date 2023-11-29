using HomeHive.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeHive.Identity;

public class HomeHiveIdentityContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public HomeHiveIdentityContext(DbContextOptions<HomeHiveIdentityContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>().Ignore(u => u.Estates);
    }
}