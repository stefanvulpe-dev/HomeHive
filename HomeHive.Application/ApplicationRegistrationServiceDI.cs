using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace HomeHive.Application;

public static class ApplicationRegistrationServiceDI
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR
            (
                cfg => cfg.RegisterServicesFromAssembly(
                    
                    Assembly.GetExecutingAssembly())
            );
    }
}