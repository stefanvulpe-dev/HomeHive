using Azure.Storage.Blobs;
using HomeHive.Application.Contracts.Caching;
using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Persistence;
using HomeHive.Infrastructure.Caching;
using HomeHive.Infrastructure.Repositories;
using HomeHive.Infrastructure.Services.BlobStorage;
using HomeHive.Infrastructure.Services.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeHive.Infrastructure;

public static class InfrastructureRegistrationDI
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<HomeHiveContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("HomeHiveConnection"),
                builder => builder.MigrationsAssembly(typeof(HomeHiveContext).Assembly.FullName)));
        services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("AuthRedisConnection");
        });
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IContractRepository, ContractRepository>();
        services.AddScoped<IEstateRepository, EstateRepository>();
        services.AddScoped<IUtilityRepository, UtilityRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IEstatePhotosRepository, EstatePhotosRepository>();
        services.AddScoped<IEstateRoomRepository, EstateRoomRepository>();
        services.AddSingleton(x => new BlobServiceClient(configuration["AzureBlobStorage:ConnectionString"]));
        services.Configure<BlobStorageOptions>(configuration.GetSection("AzureBlobStorage"));
        services.Configure<SendGridConfigurationOptions>(configuration.GetSection("SendGrid"));
        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton<IBlobStorageService, BlobStorageService>();
        return services;
    }
}