using HomeHive.UI.Utils.Responses;
using HomeHive.UI.ViewModels.Users;

namespace HomeHive.UI.Interfaces;

using TLoginResponse = Dictionary<string, string>;

public interface IAuthService
{
    Task<ApiResponse<TLoginResponse>?> Login(LoginModel loginModel);
    Task<ApiResponse?> Register(RegistrationModel registrationModel);
    Task<bool> Refresh();
    Task<bool> Logout();
    Task<string> GetAccessTokenFromBrowserStorage();
}