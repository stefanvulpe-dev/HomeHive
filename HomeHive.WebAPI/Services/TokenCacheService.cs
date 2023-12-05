using System.Security.Claims;
using HomeHive.Application.Contracts.Caching;
using HomeHive.Identity.Services.Authentication;
using Microsoft.IdentityModel.JsonWebTokens;

namespace HomeHive.WebAPI.Services;

public class TokenCacheService(IHttpContextAccessor contextAccessor, ICacheService cacheService) : ITokenCacheService
{
    public async Task<bool> IsTokenRevokedAsync(CancellationToken cancellationToken = default)
    {
        var userId = contextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;
        if (userId is null)
            return true;
        var tokenId = contextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        if (tokenId is null)
            return true;
        var isRevoked = await cacheService.GetAsync<CachedTokenData>($"{userId}:{tokenId}", cancellationToken);
        return isRevoked is null;
    }
}