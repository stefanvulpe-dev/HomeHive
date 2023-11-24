using HomeHive.Application.Persistence;
using HomeHive.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeHive.Infrastructure;

public static class InfrastructureRegistrationDI
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HomeHiveContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("HomeHiveConnection"), 
                builder => builder.MigrationsAssembly(typeof(HomeHiveContext).Assembly.FullName)));
        services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}