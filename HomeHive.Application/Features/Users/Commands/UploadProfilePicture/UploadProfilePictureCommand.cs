using HomeHive.Application.Contracts.Commands;

namespace HomeHive.Application.Features.Users.Commands.UploadProfilePicture;

public record UploadProfilePictureCommand(Guid UserId, Stream Content, string ContentType)
    : ICommand<UploadProfilePictureCommandResponse>;