using HomeHive.Application.Contracts.Commands;

namespace HomeHive.Application.Features.Users.Commands.DeleteProfilePicture;

public record DeleteProfilePictureCommand(Guid UserId) : ICommand<DeleteProfilePictureCommandResponse>;