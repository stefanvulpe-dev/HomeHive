﻿using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Estates;
using HomeHive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
    public DbSet<Utility>? Utilities { get; set; }

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
        modelBuilder.Entity<Estate>().Property(e => e.EstateType)
            .HasConversion(
                v => v.ToString(),
                v => (EstateType)Enum.Parse(typeof(EstateType), v!));
        
        modelBuilder
            .Entity<Estate>().Property(e => e.EstateCategory)
            .HasConversion(
                v => v.ToString(),
                v => (EstateCategory)Enum.Parse(typeof(EstateCategory), v!));
        
    }
}