using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace HomeHive.UI.Utils;

public class CustomHttpClientHandler(ILocalStorageService localStorageService) : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var accessToken = await localStorageService.GetItemAsync<string>("accessToken", cancellationToken);
        if (!string.IsNullOrWhiteSpace(accessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}