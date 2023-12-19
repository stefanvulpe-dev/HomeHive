using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Models;

namespace HomeHive.Application.Features.Users.Commands.UpdateEmail;

public record UpdateEmailCommand(Guid UserId, UpdateEmailModel Model) : ICommand<UpdateEmailCommandResponse>;
