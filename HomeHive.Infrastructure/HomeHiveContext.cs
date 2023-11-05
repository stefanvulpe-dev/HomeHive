﻿using HomeHive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace HomeHive.Infrastructure;

public class HomeHiveContext: DbContext
{
    public DbSet<User>? Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .SetFileProvider(new PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory))
            .AddJsonFile("appsettings.json")
            .Build();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("RelationalDatabase"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estate>().HasOne<User>(e => e.OwnerId)
            .WithMany(user => user.Estates)
            .HasForeignKey(est => est.Id);
    }
}