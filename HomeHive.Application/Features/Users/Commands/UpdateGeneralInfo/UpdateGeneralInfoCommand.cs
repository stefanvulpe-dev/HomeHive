using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Models;

namespace HomeHive.Application.Features.Users.Commands.UpdateGeneralInfo;

public record UpdateGeneralInfoCommand(Guid UserId, UpdateUserModel Data) : ICommand<UpdateGeneralInfoCommandResponse>;