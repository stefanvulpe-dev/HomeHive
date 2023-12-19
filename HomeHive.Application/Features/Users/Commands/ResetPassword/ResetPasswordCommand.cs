using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Features.Users.Commands.ResetPasswordCommand;
using HomeHive.Application.Models;

namespace HomeHive.Application.Features.Users.Commands.ResetPassword;

public record ResetPasswordCommand(Guid UserId, ResetPasswordModel ResetPasswordModel): ICommand<ResetPasswordCommandResponse>;