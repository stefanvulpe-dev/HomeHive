using System.Security.Claims;
using HomeHive.UI.Interfaces;
using HomeHive.UI.Services.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace HomeHive.UI.Utils;

public class HomeHiveAuthStateProvider(IAuthService authService) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var anonymousState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var accessToken = await authService.GetAccessTokenFromBrowserStorage();
        if (string.IsNullOrEmpty(accessToken)) return anonymousState;

        var claims = AuthService.ParseClaimsFromJwt(accessToken);
        if (claims is null) return anonymousState;

        var expires = claims.FirstOrDefault(x => x.Type.Equals("exp"))?.Value;
        if (string.IsNullOrEmpty(expires)) return anonymousState;

        var expiresDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expires));
        if (expiresDateTime.UtcDateTime <= DateTime.UtcNow)
        {
            if (!await authService.Refresh()) return anonymousState;

            accessToken = await authService.GetAccessTokenFromBrowserStorage();

            if (string.IsNullOrEmpty(accessToken)) return anonymousState;
        }

        var principal = AuthService.CreateClaimsPrincipalFromToken(accessToken);
        return new AuthenticationState(principal);
    }

    public async Task LoginAsync()
    {
        var accessToken = await authService.GetAccessTokenFromBrowserStorage();

        var principal = AuthService.CreateClaimsPrincipalFromToken(accessToken);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public async Task LogoutAsync()
    {
        await authService.Logout();
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
    }
}