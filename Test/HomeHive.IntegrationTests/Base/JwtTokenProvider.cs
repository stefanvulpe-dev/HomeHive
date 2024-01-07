using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IntegrationTests.Base;

public static class JwtTokenProvider
{
    public static string Issuer { get; } = "https://localhost:7082";
    public static SecurityKey SecurityKey { get; } =
        new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes("7567693c464441e000f6f4150eb7ff2db7449baa25e8b369dead88967e2f841b")
        );
    
    public static SigningCredentials SigningCredentials { get; } = 
        new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
    
    internal static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
}