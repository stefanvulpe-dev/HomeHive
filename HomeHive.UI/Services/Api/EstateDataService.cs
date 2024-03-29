﻿using System.Net;
using System.Net.Http.Json;
using HomeHive.Application.Features.Estates.Commands.CreateEstate;
using HomeHive.Application.Features.Estates.Commands.DeleteEstateById;
using HomeHive.Application.Features.Estates.Commands.UpdateEstate;
using HomeHive.Application.Features.Estates.Commands.UpdateEstateAvatar;
using HomeHive.Application.Features.Estates.Commands.UpdateEstatePhotos;
using HomeHive.Application.Features.Estates.Queries.GetAllEstates;
using HomeHive.Application.Features.Estates.Queries.GetEstateById;
using HomeHive.Application.Features.Estates.Queries.GetEstatePhotos;
using HomeHive.UI.Interfaces;
using HomeHive.UI.ViewModels.Estates;

namespace HomeHive.UI.Services.Api;

public class EstatesDataService(IHttpClientFactory httpClientFactory) : IEstateDataService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("HomeHive.API");

    public async Task<GetAllEstatesResponse?> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("api/v1/Estates");
        if (!responseMessage.IsSuccessStatusCode) return null;
        return await responseMessage.Content.ReadFromJsonAsync<GetAllEstatesResponse>();
    }

    public async Task<GetEstateByIdResponse?> GetById(Guid? id)
    {
        var responseMessage = await _httpClient.GetAsync($"/api/v1/Estates/{id}");
        return await responseMessage.Content.ReadFromJsonAsync<GetEstateByIdResponse>();
    }

    public async Task<CreateEstateCommandResponse?> Add(MultipartFormDataContent content)
    {
        var responseMessage = await _httpClient.PostAsync("/api/v1/Estates", content);
        return await responseMessage.Content.ReadFromJsonAsync<CreateEstateCommandResponse>();
    }

    public async Task<UpdateEstateCommandResponse?> UpdateById(Guid? id, EditEstateModel entity)
    {
        var responseMessage = await _httpClient.PutAsJsonAsync($"/api/v1/Estates/{id}", entity);
        return await responseMessage.Content.ReadFromJsonAsync<UpdateEstateCommandResponse>();
    }

    public async Task<UpdateEstatePhotosCommandResponse?> UpdatePhotos(MultipartFormDataContent content, Guid? estateId)
    {
        var responseMessage = await _httpClient.PutAsync($"/api/v1/Estates/{estateId}/Photos", content);
        return await responseMessage.Content.ReadFromJsonAsync<UpdateEstatePhotosCommandResponse>();
    }

    public async Task<DeleteEstateByIdCommandResponse?> DeleteById(Guid? id)
    {
        var responseMessage = await _httpClient.DeleteAsync($"/api/v1/Estates/{id}");
        
        if (!responseMessage.StatusCode.Equals(HttpStatusCode.NoContent)) 
            return await responseMessage.Content.ReadFromJsonAsync<DeleteEstateByIdCommandResponse>();
        
        return new DeleteEstateByIdCommandResponse
        {
            IsSuccess = true,
            Message = "Estate deleted successfully"
        };
    }

    public async Task<GetEstatePhotosQueryResponse?> GetEstatePhotos(Guid? estateId)
    {
        var responseMessage = await _httpClient.GetAsync($"/api/v1/Estates/{estateId}/Photos");
        return await responseMessage.Content.ReadFromJsonAsync<GetEstatePhotosQueryResponse>();
    }

    public async Task<UpdateEstateAvatarCommandResponse?> UpdateAvatar(MultipartFormDataContent content, Guid? estateId)
    {
        var responseMessage = await _httpClient.PutAsync($"/api/v1/Estates/{estateId}/Avatar", content);
        return await responseMessage.Content.ReadFromJsonAsync<UpdateEstateAvatarCommandResponse>();
    }
}