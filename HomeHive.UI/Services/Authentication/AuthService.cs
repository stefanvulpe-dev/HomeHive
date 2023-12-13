using HomeHive.UI.Utils.Responses;
using HomeHive.UI.ViewModels.Users;

namespace HomeHive.UI.Services.Authentication;

using TLoginResponse = Dictionary<string, string>;

public class AuthService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("HomeHive.API");

    public async Task<BaseApiResponse?> Register(RegistrationModel registrationModel)
    {
        var result = await _httpClient.PostAsJsonAsync("api/v1/Authentication/register", registrationModel);
        return await result.Content.ReadFromJsonAsync<BaseApiResponse>();
    }

    public async Task<ApiResponse<TLoginResponse>?> Login(LoginModel loginModel)
    {
        var result = await _httpClient.PostAsJsonAsync("api/v1/Authentication/login", loginModel);
        return await result.Content.ReadFromJsonAsync<ApiResponse<TLoginResponse>>();
    }

    public async Task Logout()
    {
        await _httpClient.PostAsync("/Authentication/logout", null);
    }
}