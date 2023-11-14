using HomeHive.Application.Abstractions;

namespace HomeHive.Application.Features.Users.Commands.DeleteUserById;

public record DeleteUserByIdCommand(Guid Id) : ICommand<DeleteUserByIdCommandResponse>;