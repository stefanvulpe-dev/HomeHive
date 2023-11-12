using HomeHive.Application.Abstractions;

namespace HomeHive.Application.Features.Commands.DeleteUser;

public record DeleteUserCommand(string Email) : ICommand<DeleteUserCommandResponse>;
