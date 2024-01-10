using System.Net.Http.Json;
using HomeHive.Application.Features.Contracts.Commands.CreateContract;
using HomeHive.UI.Interfaces;
using HomeHive.UI.ViewModels.Estates;

namespace HomeHive.UI.Services.Api;

public class ContractDataService(IHttpClientFactory httpClientFactory) : IContractDataService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("HomeHive.API");
    
    public async Task<CreateContractCommandResponse?> Add(EstateBuyModel? estateBuyModel)
    {
        if (estateBuyModel != null)
        {
            estateBuyModel.StartDate = estateBuyModel.StartDate?.ToUniversalTime();
            estateBuyModel.EndDate = estateBuyModel.EndDate?.ToUniversalTime();
        }
        var response = await _httpClient.PostAsJsonAsync("/api/v1/Contracts", estateBuyModel);
        return await response.Content.ReadFromJsonAsync<CreateContractCommandResponse>();
    }
}