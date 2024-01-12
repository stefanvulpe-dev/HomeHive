using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using HomeHive.UI.Services.Authentication;
using HomeHive.UI.Utils.Responses;
using Microsoft.AspNetCore.Components;

namespace HomeHive.UI.Utils;

public class CustomHttpClientHandler(ILocalStorageService 
        localStorageService, NavigationManager navigationManager)
    : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var accessToken =
            await localStorageService.GetItemAsync<string>("accessToken",
                cancellationToken);
        if (!string.IsNullOrWhiteSpace(accessToken))
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var refreshToken =
                await localStorageService.GetItemAsync<string>("refreshToken",
                    cancellationToken);
            var refreshTokenRequest = new HttpRequestMessage(HttpMethod.Post,
                "http://localhost:5144/api/v1/Authentication/refresh");
            refreshTokenRequest.Headers.Add("refreshToken", refreshToken);
            var refreshTokenResponse =
                await base.SendAsync(refreshTokenRequest, cancellationToken);

            if (refreshTokenResponse.StatusCode == HttpStatusCode.OK)
            {
                var tokens = await refreshTokenResponse.Content
                    .ReadFromJsonAsync<ApiResponse<RefreshTokensResponse>>(
                        cancellationToken);
                await localStorageService.SetItemAsync("accessToken",
                    tokens?.Value?.AccessToken,
                    cancellationToken);
                await localStorageService.SetItemAsync("refreshToken", tokens?
                        .Value?.RefreshToken,
                    cancellationToken);
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer",
                        tokens?.Value?.AccessToken);
            }
            else
            {
                await localStorageService.RemoveItemAsync("accessToken",
                    cancellationToken);
                await localStorageService.RemoveItemAsync("refreshToken",
                    cancellationToken);
                navigationManager.NavigateTo("/login", true);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}