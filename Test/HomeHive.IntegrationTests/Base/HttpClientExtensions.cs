using System.Net.Http.Headers;

namespace IntegrationTests.Base;

public static class HttpClientExtensions
{
    public static HttpClient WithJwtBearerToken(this HttpClient client, Action<JwtTokenBuilder> configure)
    {
        var token = new JwtTokenBuilder();
        configure(token);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Build());
        return client;
    }
}