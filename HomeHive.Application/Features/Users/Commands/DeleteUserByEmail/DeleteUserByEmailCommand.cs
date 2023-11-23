using HomeHive.Application.Abstractions;

namespace HomeHive.Application.Features.Users.Commands.DeleteUserByEmail;

public record DeleteUserByEmailCommand(string Email) : ICommand<DeleteUserByEmailCommandResponse>;
