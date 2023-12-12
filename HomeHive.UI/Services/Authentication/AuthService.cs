using HomeHive.UI.Services.Api;
using HomeHive.UI.ViewModels.Users;

namespace HomeHive.UI.Services.Authentication;

using TLoginResponse = Dictionary<string, string>;

public class AuthService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("HomeHive.API");

    public async Task<ApiResponse?> Register(RegistrationModel registrationModel)
    {
        var result = await _httpClient.PostAsJsonAsync("api/v1/Authentication/register", registrationModel);
        return await result.Content.ReadFromJsonAsync<ApiResponse>();
    }

    public async Task<ApiResponse<TLoginResponse>?> Login(LoginModel loginModel)
    {
        var result = await _httpClient.PostAsJsonAsync("api/v1/Authentication/login", loginModel);
        return await result.Content.ReadFromJsonAsync<ApiResponse<Dictionary<string, string>>>();
    }

    public async Task Logout()
    {
        await _httpClient.PostAsync("/Authentication/logout", null);
    }
}