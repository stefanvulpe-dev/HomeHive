using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomeHive.Application.Contracts.Identity;
using HomeHive.Application.Models;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;
using HomeHive.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace HomeHive.Identity.Services;

public class AuthService(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager,
        IConfiguration configuration)
    : IAuthService
{
    public async Task<Result> Register(RegistrationModel model, string role)
    {
        var identityUser = new User
        {
            Email = model.Email,
            UserName = model.UserName,
            FirstName = model.FirstName,
            LastName = model.LastName,
            ProfilePicture = model.ProfilePicture,
            PhoneNumber = model.PhoneNumber
        };

        var result = await userManager.CreateAsync(identityUser, model.Password!);

        if (!result.Succeeded)
            return Result.Failure("Failed to create the identity user", result.Errors.Select(error => error.Description).ToList());

        if (!await roleManager.RoleExistsAsync(role))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            if (!roleResult.Succeeded)
                return Result.Failure("Failed to create the role", roleResult.Errors.Select(error => error.Description).ToList());
        }

        if (await roleManager.RoleExistsAsync(role))
        {
            var addToRoleResult = await userManager.AddToRoleAsync(identityUser, role);
            if (!addToRoleResult.Succeeded)
                return Result.Failure("Failed to add the user to the role", addToRoleResult.Errors.Select(error => error.Description).ToList());
        }

        return Result.Success("User created successfully!");
    }

    public async Task<Result<string>> Login(LoginModel model)
    {
        var user = await userManager.FindByNameAsync(model.UserName!);

        if (user == null)
            return Result<string>.Failure("Invalid credentials");

        if (!await userManager.CheckPasswordAsync(user, model.Password!))
            return Result<string>.Failure("Invalid credentials");

        var userRoles = await userManager.GetRolesAsync(user);
        
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = GenerateToken(authClaims);

        return Result<string>.Success(token);
    }

    private string GenerateToken(IEnumerable<Claim> authClaims)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = configuration["JWT:ValidIssuer"],
            Audience = configuration["JWT:ValidAudience"],
            Subject = new ClaimsIdentity(authClaims),
            Expires = DateTime.Now.AddHours(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!)),
                    SecurityAlgorithms.HmacSha256Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}