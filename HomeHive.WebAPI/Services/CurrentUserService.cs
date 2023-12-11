using System.Security.Claims;
using HomeHive.Application.Contracts.Interfaces;

namespace HomeHive.WebAPI.Services;

public class CurrentUserService(IHttpContextAccessor contextAccessor) : ICurrentUserService
{
    public Guid GetCurrentUserId()
    {
        return contextAccessor.HttpContext != null
            ? Guid.Parse(contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!)
            : Guid.Empty;
    }

    public string GetCurrentUserName()
    {
        return contextAccessor.HttpContext != null
            ? contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name)!
            : null!;
    }
}