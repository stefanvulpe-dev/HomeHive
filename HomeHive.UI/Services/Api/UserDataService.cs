using System.Net.Http.Json;
using HomeHive.Application.Features.Users.Commands.ResetPasswordCommand;
using HomeHive.Application.Features.Users.Commands.UpdateEmail;
using HomeHive.Application.Features.Users.Commands.UpdateGeneralInfo;
using HomeHive.Application.Features.Users.Commands.UploadProfilePicture;
using HomeHive.Application.Features.Users.Queries.GetResetToken;
using HomeHive.Application.Features.Users.Queries.GetUserById;
using HomeHive.Application.Features.Users.Queries.GetUserGeneralInfo;
using HomeHive.UI.Interfaces;
using HomeHive.UI.ViewModels.Users;

namespace HomeHive.UI.Services.Api;

public class UserDataService(IHttpClientFactory httpClientFactory): IUserDataService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("HomeHive.API");
    
    public async Task<string> GetProfilePicture()
    {
        var byteArray = await _httpClient.GetByteArrayAsync("/api/v1/Users/profile-picture"); 
        var base64 = Convert.ToBase64String(byteArray);
        return $"data:image/jpeg;base64,{base64}";
    }
    
    public async Task<GetUserByIdResponse?> GetUserInfo()
    {
        var responseMessage = await _httpClient.GetAsync("/api/v1/Users");
        return await responseMessage.Content.ReadFromJsonAsync<GetUserByIdResponse>();
    }

    public async Task<ResetPasswordCommandResponse?> ResetPassword(ChangePasswordModel request)
    {
        var responseMessage = await _httpClient.PutAsJsonAsync("/api/v1/Users/reset-password", request);
        return await responseMessage.Content.ReadFromJsonAsync<ResetPasswordCommandResponse>();
    }
    
    public async Task<GetResetTokenQueryResponse?> GetResetToken(string purpose)
    {
        var responseMessage = await _httpClient.GetAsync($"/api/v1/Users/reset-token/{purpose}");
        return await responseMessage.Content.ReadFromJsonAsync<GetResetTokenQueryResponse>();
    }
    
    public async Task<UpdateEmailCommandResponse?> UpdateEmail(UpdateEmailModel request)
    {
        var responseMessage = await _httpClient.PutAsJsonAsync("/api/v1/Users/update-email", request);
        return await responseMessage.Content.ReadFromJsonAsync<UpdateEmailCommandResponse>();
    }
    
    public async Task<UpdateGeneralInfoCommandResponse?> UpdateGeneralInfo(UserModel user)
    {
        var responseMessage = await _httpClient.PutAsJsonAsync("/api/v1/Users", user);
        return await responseMessage.Content.ReadFromJsonAsync<UpdateGeneralInfoCommandResponse>();
    }
    
    public async Task<UploadProfilePictureCommandResponse?> UploadProfilePicture(MultipartFormDataContent content)
    {
        var responseMessage = await _httpClient.PutAsync("/api/v1/Users/profile-picture", content);
        return await responseMessage.Content.ReadFromJsonAsync<UploadProfilePictureCommandResponse>();
    }

    public async Task<GetUserGeneralInfoQueryResponse?> GetUserGeneralInfo(Guid userId)
    {
        var responseMessage = await _httpClient.GetAsync($"/api/v1/Users/General/{userId}");
        return await responseMessage.Content.ReadFromJsonAsync<GetUserGeneralInfoQueryResponse>();
    }
}