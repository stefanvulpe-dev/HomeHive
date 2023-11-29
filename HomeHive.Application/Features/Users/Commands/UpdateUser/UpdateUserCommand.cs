using HomeHive.Application.Abstractions;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(Guid UserId, UserData UserData) : ICommand<UpdateUserCommandResponse>;