using HomeHive.Application.Abstractions;

namespace HomeHive.Application.Features.Commands.DeleteUserById;

public record DeleteUserByIdCommand(Guid Id) : ICommand<DeleteUserByIdCommandResponse>;