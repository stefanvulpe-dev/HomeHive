using System.Security.Claims;
using HomeHive.Application.Contracts.Caching;
using HomeHive.Application.Contracts.Identity;
using HomeHive.Application.Models;
using HomeHive.Domain.Common;
using HomeHive.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;

namespace HomeHive.Identity.Services.Authentication;

public class AuthService(
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    ICacheService cacheService,
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration)
    : IAuthService
{
    public async Task<Result> Register(RegistrationModel model, string role)
    {
        var userRegistrationValidator = new UserRegistrationValidator();
        
        var validationResult = await userRegistrationValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
            return Result.Failure("Failed to create identity user",
                validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage));
        
        var identityUser = new User
        {
            Email = model.Email,
            UserName = model.UserName,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber
        };
        
        var result = await userManager.CreateAsync(identityUser, model.Password!);

        if (!result.Succeeded)
        {
            var errorDictionary = new Dictionary<string, string>();
            foreach (var error in result.Errors)
            {
                if (error.Description.Contains("Email"))
                    errorDictionary.Add("Email", error.Description);
                else if (error.Description.Contains("Username"))
                    errorDictionary.Add("UserName", error.Description);
                else if (error.Description.Contains("Password"))
                    errorDictionary.Add("Password", error.Description);
            }
            return Result.Failure("Failed to create identity user", errorDictionary);
        }

        if (!await roleManager.RoleExistsAsync(role))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            if (!roleResult.Succeeded)
            {
                var errorDictionary = new Dictionary<string, string>();
                foreach (var error in result.Errors)
                {
                    if (error.Description.Contains("Name"))
                        errorDictionary.Add("Name", error.Description);
                }
                return Result.Failure("Failed to create identity role", errorDictionary);
            }
        }

        if (await roleManager.RoleExistsAsync(role))
        {
            var addToRoleResult = await userManager.AddToRoleAsync(identityUser, role);
            if (!addToRoleResult.Succeeded)
            {
                var errorDictionary = new Dictionary<string, string>();
                foreach (var error in result.Errors)
                {
                    if (error.Description.Contains("Role"))
                        errorDictionary.Add("Role", error.Description);
                }
                return Result.Failure("Failed to add the user to the role", errorDictionary);
            }
        }

        return Result.Success("User created successfully!");
    }

    public async Task<LoginResult> Login(LoginModel model)
    {
        var user = await userManager.FindByNameAsync(model.UserName!);

        if (user == null)
            return LoginResult.Failure("Invalid credentials");

        if (!await userManager.CheckPasswordAsync(user, model.Password!))
            return LoginResult.Failure("Invalid credentials");

        var userRoles = await userManager.GetRolesAsync(user);

        var (accessTokenId, accessToken, refreshToken, accessTokenExpiration) =
            AuthServiceUtils.GenerateTokens(user, userRoles, configuration);

        var tokenData = new { AccessToken = accessToken };
        await cacheService.SetAsync($"{user.Id.ToString()}:{accessTokenId}",
            tokenData, accessTokenExpiration - DateTime.UtcNow);

        return LoginResult.Success("User logged in successfully!", accessToken, refreshToken);
    }

    public async Task<LoginResult> Refresh()
    {
        var token = httpContextAccessor.HttpContext?.Request.Headers["refreshToken"].ToString();
        if (string.IsNullOrEmpty(token))
            return LoginResult.Failure("Failed to refresh", new Dictionary<string, string?>
            {
                { "refreshToken", "Missing refresh token" }
            });

        var refreshToken = await AuthServiceUtils.ValidateToken(token, configuration);
        if (refreshToken == null)
            return LoginResult.Failure("Failed to refresh", new Dictionary<string, string?>
            {
                { "refreshToken", "Invalid refresh token" }
            });

        var userIdClaim = refreshToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");
        if (userIdClaim == null)
            return LoginResult.Failure("Failed to refresh", new Dictionary<string, string?>
            {
                { "refreshToken", "Missing user id claim" }
            });

        var refreshTokenIdClaim = refreshToken.Claims.FirstOrDefault(claim => claim.Type == "jti");
        if (refreshTokenIdClaim == null)
            return LoginResult.Failure("Failed to refresh", new Dictionary<string, string?>
            {
                { "refreshToken", "Invalid refresh token id" }
            });

        var user = await userManager.FindByIdAsync(userIdClaim.Value);
        if (user == null)
            return LoginResult.Failure("Failed to refresh", new Dictionary<string, string?>
            {
                { "refreshToken", "Invalid user id" }
            });

        var userRoles = await userManager.GetRolesAsync(user);

        var (accessTokenId, accessToken, newRefreshToken, accessTokenExpiration) =
            AuthServiceUtils.GenerateTokens(user, userRoles, configuration);

        var tokenData = new { AccessToken = accessToken };
        await cacheService.SetAsync($"{user.Id.ToString()}:{accessTokenId}", tokenData,
            accessTokenExpiration - DateTime.UtcNow);

        return LoginResult.Success("User logged in successfully!", accessToken, newRefreshToken);
    }

    public async Task<Result> Logout()
    {
        var accessTokenId = httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Jti);
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        await cacheService.RemoveAsync($"{userId}:{accessTokenId}");

        return Result.Success("User logged out successfully!");
    }
}