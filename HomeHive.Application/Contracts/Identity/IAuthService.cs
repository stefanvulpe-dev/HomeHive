using HomeHive.Application.Models;
using HomeHive.Domain.Common;

namespace HomeHive.Application.Contracts.Identity;

using TDictResponse = Dictionary<string, string>;

public interface IAuthService
{
    Task<Result> Register(RegistrationModel model, string role);
    Task<Result<TDictResponse>> Login(LoginModel model);
    Task<Result<TDictResponse>> Refresh();
    Task<Result> Logout();
}