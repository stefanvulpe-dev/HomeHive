using HomeHive.Application.Contracts.Caching;
using HomeHive.Application.Persistence;
using HomeHive.Infrastructure.Caching;
using HomeHive.Infrastructure.Repositories;
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
        return services;
    }
}