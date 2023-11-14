using HomeHive.Application.Abstractions;

namespace HomeHive.Application.Features.Commands.DeleteUserByEmail;

public record DeleteUserByEmailCommand(string Email) : ICommand<DeleteUserByEmailCommandResponse>;
