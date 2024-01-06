using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Users.Commands.UploadProfilePicture;

public class
    UploadProfilePictureCommandHandler(IBlobStorageService blobStorageService, IUserRepository userRepository)
    : ICommandHandler<UploadProfilePictureCommand,
        UploadProfilePictureCommandResponse>
{
    public async Task<UploadProfilePictureCommandResponse> Handle(UploadProfilePictureCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new UploadProfilePictureCommandValidator(userRepository);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors
                .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());
            return new UploadProfilePictureCommandResponse
            {
                IsSuccess = false,
                Message = "Validation errors occurred",
                ValidationsErrors = validationErrors
            };
        }

        var blobName = Guid.NewGuid().ToString();

        var uploadResult = await blobStorageService.UploadBlobAsync(blobName, command.Content, cancellationToken);

        if (!uploadResult)
            return new UploadProfilePictureCommandResponse { IsSuccess = false, Message = "Error uploading blob" };

        var user = validator.User!;

        if (user.ProfilePicture != null)
            await blobStorageService.DeleteBlobAsync(user.ProfilePicture, cancellationToken);

        user.ProfilePicture = blobName;

        var updateResult = await userRepository.UpdateAsync(user);

        return updateResult.IsSuccess
            ? new UploadProfilePictureCommandResponse
                { IsSuccess = true, Message = "Profile picture updated successfully" }
            : new UploadProfilePictureCommandResponse { IsSuccess = false, Message = "Error updating profile picture" };
    }
}