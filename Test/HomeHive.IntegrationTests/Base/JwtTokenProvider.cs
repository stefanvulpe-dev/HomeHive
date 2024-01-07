using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IntegrationTests.Base;

public static class JwtTokenProvider
{
    public static string Issuer { get; } = "Sample_Auth_Server";
    public static SecurityKey SecurityKey { get; } =
        new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IlJhcmUyMDAyIiwibmFtZWlkIjoiODBjZGIzNTAtNjNlMS00NWM0LTg4NmEtOWJmZWRlMjU0ZGJhIiwianRpIjoiMmQxMTUwMjUtNGFjZi00M2ZlLThkMjYtYTY1YTRjYWFlNDViIiwicm9sZSI6IlVzZXIiLCJuYmYiOjE3MDQ1NjY1MTMsImV4cCI6MTcwNDU2NzExMywiaWF0IjoxNzA0NTY2NTEzLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MTQ0IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTE0NCJ9.P6D-mHYwKaK9MDCkTPQl2pqibSukOaYKdJAUlYEtijQ")
        );
    
    public static SigningCredentials SigningCredentials { get; } = 
        new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
    
    internal static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
}