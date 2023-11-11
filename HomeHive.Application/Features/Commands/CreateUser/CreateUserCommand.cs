using HomeHive.Application.Abstractions;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Commands.CreateUser;

public record CreateUserCommand(UserData UserData) : ICommand<CreateUserCommandResponse>;