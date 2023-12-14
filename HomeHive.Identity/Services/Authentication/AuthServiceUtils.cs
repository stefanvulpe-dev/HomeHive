using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomeHive.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HomeHive.Identity.Services.Authentication;

public record TokenData(string AccessTokenId, string AccessToken, string RefreshToken, DateTime AccessTokenExpiration);

public record CachedTokenData(string AccessToken);

public static class AuthServiceUtils
{
    private static string GenerateToken(User user, IEnumerable<Claim> claims, IConfiguration configuration,
        DateTime expires = default)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = configuration["JWT:ValidIssuer"],
            Audience = configuration["JWT:ValidAudience"],
            Subject = new ClaimsIdentity(claims),
            Expires = expires == default ? DateTime.UtcNow.AddMinutes(30) : expires,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!)),
                    SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static TokenData GenerateTokens(User user, IEnumerable<string> userRoles, IConfiguration configuration)
    {
        var accessTokenId = Guid.NewGuid().ToString();

        var accessTokenClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, accessTokenId)
        };

        accessTokenClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(10);
        var accessToken = GenerateToken(user, accessTokenClaims, configuration, accessTokenExpiration);

        var refreshTokenClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var refreshToken =
            GenerateToken(user, refreshTokenClaims, configuration, DateTime.UtcNow.AddDays(7));

        return new TokenData(accessTokenId, accessToken, refreshToken, accessTokenExpiration);
    }


    public static async Task<JwtSecurityToken?> ValidateToken(string token, IConfiguration configuration)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = configuration["JWT:ValidIssuer"],
            ValidAudience = configuration["JWT:ValidAudience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!))
        });

        return !tokenValidationResult.IsValid ? null : tokenHandler.ReadJwtToken(token);
    }
}