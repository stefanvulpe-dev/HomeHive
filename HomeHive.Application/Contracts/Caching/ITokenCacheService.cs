namespace HomeHive.Application.Contracts.Caching;

public interface ITokenCacheService
{
    Task<bool> IsTokenRevokedAsync(CancellationToken cancellationToken = default);
}