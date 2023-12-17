using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Users.Commands.DeleteProfilePicture;

public class DeleteProfilePictureCommandHandler(IUserRepository userRepository, IBlobStorageService blobStorageService)
    : ICommandHandler<DeleteProfilePictureCommand, DeleteProfilePictureCommandResponse>
{
    public async Task<DeleteProfilePictureCommandResponse> Handle(DeleteProfilePictureCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new DeleteProfilePictureCommandValidator(userRepository);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors
                .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());
            
            return new DeleteProfilePictureCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = validationErrors
            };
        }

        var user = validator.User!;

        await blobStorageService.DeleteBlobAsync(user.ProfilePicture!, cancellationToken);

        user.ProfilePicture = null;

        var updateResult = await userRepository.UpdateAsync(user);

        if (!updateResult.IsSuccess)
            return new DeleteProfilePictureCommandResponse
            {
                IsSuccess = false,
                Message = "An error occurred while deleting the user profile picture"
            };

        return new DeleteProfilePictureCommandResponse
        {
            IsSuccess = true
        };
    }
}