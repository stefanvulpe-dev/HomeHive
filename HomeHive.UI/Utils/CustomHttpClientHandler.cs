using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using HomeHive.UI.Utils.Responses;

namespace HomeHive.UI.Utils;

public class CustomHttpClientHandler(ILocalStorageService localStorageService, string baseUrl) : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var accessToken = await localStorageService.GetItemAsync<string>("accessToken", cancellationToken);
        if (!string.IsNullOrWhiteSpace(accessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var refreshToken = await localStorageService.GetItemAsync<string>("refreshToken", cancellationToken);
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var refreshRequest = new HttpRequestMessage(HttpMethod.Post,
                    new Uri($"{baseUrl}/api/v1/Authentication/refresh"));

                refreshRequest.Headers.Add("refreshToken", refreshToken);

                var refreshResponse = await base.SendAsync(refreshRequest, cancellationToken);

                if (refreshResponse.StatusCode == HttpStatusCode.OK)
                {
                    var refreshTokensResponse =
                        await refreshResponse.Content.ReadFromJsonAsync<ApiResponse<RefreshTokensResponse>>(
                            cancellationToken);

                    if (refreshTokensResponse is { IsSuccess: true })
                    {
                        await localStorageService.SetItemAsync("accessToken", refreshTokensResponse.Value?.AccessToken,
                            cancellationToken);
                        await localStorageService.SetItemAsync("refreshToken",
                            refreshTokensResponse.Value?.RefreshToken, cancellationToken);

                        request.Headers.Authorization =
                            new AuthenticationHeaderValue("Bearer", refreshTokensResponse.Value?.AccessToken);

                        return await base.SendAsync(request, cancellationToken);
                    }
                }
            }
        }

        return response;
    }
}

public record RefreshTokensResponse(string AccessToken, string RefreshToken);