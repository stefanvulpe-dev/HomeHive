using System.Threading.Tasks;
using HomeHive.Application.Models;
using HomeHive.Domain.Common;

namespace HomeHive.Application.Contracts.Identity;

public interface IAuthService
{
    Task<Result> Register(RegistrationModel model, string role);
    Task<Result<string>> Login(LoginModel model);
}