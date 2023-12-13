using System.Net.Http.Headers;
using HomeHive.Application.Features.Estates.Commands.DeleteEstateById;
using HomeHive.Application.Features.Estates.Commands.UpdateEstate;
using HomeHive.Application.Features.Estates.Queries.GetAllEstates;
using HomeHive.Application.Features.Estates.Queries.GetEstateById;
using HomeHive.Application.Features.Users.Commands.CreateEstate;
using HomeHive.UI.Utils.Interfaces;
using HomeHive.UI.ViewModels.Estates;

namespace HomeHive.UI.Services.Api;

public class EstatesDataService(IHttpClientFactory httpClientFactory) : IEstateDataService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("HomeHive.API");

    public void SetAccessToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<GetAllEstatesResponse?> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("/api/v1/Estates");
        return await responseMessage.Content.ReadFromJsonAsync<GetAllEstatesResponse>();
    }

    public async Task<GetEstateByIdResponse?> GetById(Guid id)
    {
        var responseMessage = await _httpClient.GetAsync($"/api/v1/Estates/{id}");
        return await responseMessage.Content.ReadFromJsonAsync<GetEstateByIdResponse>();
    }

    public async Task<CreateEstateCommandResponse?> Add(EstateModel entity)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync("/api/v1/Estates", entity);
        return await responseMessage.Content.ReadFromJsonAsync<CreateEstateCommandResponse>();
    }

    public async Task<UpdateEstateCommandResponse?> UpdateById(Guid id, EstateModel entity)
    {
        var responseMessage = await _httpClient.PutAsJsonAsync($"/api/v1/Estates/{id}", entity);
        return await responseMessage.Content.ReadFromJsonAsync<UpdateEstateCommandResponse>();
    }

    public async Task<DeleteEstateByIdCommandResponse?> DeleteById(Guid id)
    {
        var responseMessage = await _httpClient.DeleteAsync($"/api/v1/Estates/{id}");
        return await responseMessage.Content.ReadFromJsonAsync<DeleteEstateByIdCommandResponse>();
    }
}