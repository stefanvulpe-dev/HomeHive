using HomeHive.Application.Features.Users.Commands.ResetPasswordCommand;
using HomeHive.Application.Features.Users.Commands.UpdateEmail;
using HomeHive.Application.Features.Users.Commands.UpdateGeneralInfo;
using HomeHive.Application.Features.Users.Commands.UploadProfilePicture;
using HomeHive.Application.Features.Users.Queries.GetResetToken;
using HomeHive.Application.Features.Users.Queries.GetUserById;
using HomeHive.Application.Features.Users.Queries.GetUserGeneralInfo;
using HomeHive.UI.ViewModels.Users;

namespace HomeHive.UI.Interfaces;

public interface IUserDataService
{
    Task<string> GetProfilePicture();
    Task<GetUserByIdResponse?> GetUserInfo();
    Task<ResetPasswordCommandResponse?> ResetPassword(ChangePasswordModel request);
    Task<GetResetTokenQueryResponse?> GetResetToken(string purpose);
    Task<UpdateEmailCommandResponse?> UpdateEmail(UpdateEmailModel request);
    Task<UpdateGeneralInfoCommandResponse?> UpdateGeneralInfo(UserModel user);
    Task<UploadProfilePictureCommandResponse?> UploadProfilePicture(MultipartFormDataContent content);
    Task<GetUserGeneralInfoQueryResponse?> GetUserGeneralInfo(Guid userId);
}