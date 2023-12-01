using System.Security.Claims;
using HomeHive.Application.Contracts.Interfaces;

namespace HomeHive.WebAPI.Services;

public class EntityModifiedByByTrackingService(IHttpContextAccessor contextAccessor) : IEntityModifiedByTrackingService
{
    public string GetCurrentUserName()
    {
        return contextAccessor.HttpContext != null
            ? contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name)!
            : null!;
    }
}