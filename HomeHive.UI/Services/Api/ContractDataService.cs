using System.Net.Http.Json;
using HomeHive.Application.Features.Contracts.Commands.CreateContract;
using HomeHive.Application.Features.Contracts.Commands.UpdateContract;
using HomeHive.Application.Features.Contracts.Queries.GetAllContractsByOwnerId;
using HomeHive.Application.Features.Contracts.Queries.GetAllContractsByUserId;
using HomeHive.UI.Interfaces;
using HomeHive.UI.ViewModels.Contracts;

namespace HomeHive.UI.Services.Api;

public class ContractDataService(IHttpClientFactory httpClientFactory)
    : IContractDataService
{
    private readonly HttpClient _httpClient =
        httpClientFactory.CreateClient("HomeHive.API");

    public async Task<CreateContractCommandResponse?> Add(
        CreateContractViewModel? estateBuyModel)
    {
        if (estateBuyModel != null)
        {
            estateBuyModel.StartDate =
                estateBuyModel.StartDate?.ToUniversalTime();
            estateBuyModel.EndDate = estateBuyModel.EndDate?.ToUniversalTime();
        }

        var response =
            await _httpClient.PostAsJsonAsync("/api/v1/Contracts",
                estateBuyModel);
        return await response.Content.ReadFromJsonAsync<
            CreateContractCommandResponse>();
    }

    public async Task<GetAllContractsByUserIdResponse?>
        GetUserContracts()
    {
        var response = await _httpClient.GetAsync("/api/v1/Contracts/user/");
        return await response.Content
            .ReadFromJsonAsync<GetAllContractsByUserIdResponse>();
    }

    public async Task<GetAllContractsByOwnerIdQueryResponse?>
        GetAllContractsByOwnerId(Guid ownerId)
    {
        var response =
            await _httpClient.GetAsync($"/api/v1/Contracts/All/{ownerId}");
        return await response.Content
            .ReadFromJsonAsync<GetAllContractsByOwnerIdQueryResponse>();
    }

    public async Task<UpdateContractCommandResponse?> UpdateStatus(
        Guid? contractId, string? status)
    {
        var response = await _httpClient.PutAsJsonAsync
            ($"/api/v1/Contracts/{contractId}", new { status });
        return await response.Content
            .ReadFromJsonAsync<UpdateContractCommandResponse>();
    }
}