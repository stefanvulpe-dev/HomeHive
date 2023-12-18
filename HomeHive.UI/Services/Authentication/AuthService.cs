using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using HomeHive.UI.Interfaces;
using HomeHive.UI.Utils.Responses;
using HomeHive.UI.ViewModels.Users;

namespace HomeHive.UI.Services.Authentication;

using TLoginResponse = Dictionary<string, string>;

public class AuthService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService) : IAuthService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("HomeHive.API");

    public async Task<ApiResponse?> Register(RegistrationModel registrationModel)
    {
        var result = await _httpClient.PostAsJsonAsync("api/v1/Authentication/register", registrationModel);
        return await result.Content.ReadFromJsonAsync<ApiResponse>();
    }

    public async Task<bool> Refresh()
    {
        var refreshToken = await localStorageService.GetItemAsync<string>("refreshToken");

        var request =
            new HttpRequestMessage(HttpMethod.Post, "api/v1/Authentication/refresh");
        request.Headers.Add("refreshToken", refreshToken);

        var result = await _httpClient.SendAsync(request);

        var refreshTokensResponse = await result.Content.ReadFromJsonAsync<ApiResponse<RefreshTokensResponse>>();

        if (refreshTokensResponse is not { IsSuccess: true }) return false;

        await PersistTokensToBrowserStorage(refreshTokensResponse.Value!.AccessToken,
            refreshTokensResponse.Value!.RefreshToken);

        return true;
    }

    public async Task<bool> Logout()
    {
        var responseMessage = await _httpClient.DeleteAsync("api/v1/Authentication/logout");
        return responseMessage.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task<string> GetAccessTokenFromBrowserStorage()
    {
        return await localStorageService.GetItemAsync<string>("accessToken");
    }

    public async Task<ApiResponse<TLoginResponse>?> Login(LoginModel loginModel)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync("api/v1/Authentication/login", loginModel);

        if (!responseMessage.IsSuccessStatusCode) return null;

        var loginResult = await responseMessage.Content.ReadFromJsonAsync<ApiResponse<TLoginResponse>>();
        if (loginResult is not { IsSuccess: true }) return null;

        await PersistTokensToBrowserStorage(loginResult.Value!["accessToken"], loginResult.Value!["refreshToken"]);

        return loginResult;
    }

    private async Task PersistTokensToBrowserStorage(string accessToken, string refreshToken)
    {
        await localStorageService.SetItemAsync("accessToken", accessToken);
        await localStorageService.SetItemAsync("refreshToken", refreshToken);
    }

    public static IEnumerable<Claim>? ParseClaimsFromJwt(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (!tokenHandler.CanReadToken(accessToken)) return null;

        var jwtSecurityToken = tokenHandler.ReadJwtToken(accessToken);

        return jwtSecurityToken.Claims;
    }

    public static ClaimsPrincipal CreateClaimsPrincipalFromToken(string token)
    {
        var claims = ParseClaimsFromJwt(token);

        if (claims is null) return new ClaimsPrincipal(new ClaimsIdentity());

        var enumerable = claims.ToList();
        var userId = enumerable.FirstOrDefault(x => x.Type.Equals("nameid"))?.Value;
        var userName = enumerable.FirstOrDefault(x => x.Type.Equals("unique_name"))?.Value;
        var tokenId = enumerable.FirstOrDefault(x => x.Type.Equals("jti"))?.Value;

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(tokenId))
            return new ClaimsPrincipal(new ClaimsIdentity());

        var userRoles = enumerable.Where(x => x.Type.Equals("role")).Select(x => x.Value).ToList();

        if (userRoles.Count == 0) return new ClaimsPrincipal(new ClaimsIdentity());


        var claimsIdentity = new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId!),
            new(ClaimTypes.Name, userName!),
            new(JwtRegisteredClaimNames.Jti, tokenId!)
        }, "Bearer");

        claimsIdentity.AddClaims(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        return new ClaimsPrincipal(claimsIdentity);
    }
}

public record RefreshTokensResponse(string AccessToken, string RefreshToken);