using System;
using HomeHive.Application.Contracts.Commands;

namespace HomeHive.Application.Features.Users.Commands.DeleteUserById;

public record DeleteUserByIdCommand(Guid UserId) : ICommand<DeleteUserByIdCommandResponse>;