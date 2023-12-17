using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Features.Users.Commands.ResetPasswordCommand;
using HomeHive.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeHive.Application.Features.Users.Commands.ResetPassword;

public class ResetPasswordCommandHandler(UserManager<User> userManager)
    : ICommandHandler<ResetPasswordCommand, ResetPasswordCommandResponse>
{
    public async Task<ResetPasswordCommandResponse> Handle(ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var validator = new ResetPasswordCommandValidator(userManager);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return new ResetPasswordCommandResponse
            {
                IsSuccess = false,
                Message = "Reset password failed.",
                ValidationsErrors = validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage)
            };

        var user = validator.User!;
        
        if (user.ResetPasswordToken != request.ResetPasswordModel.ResetPasswordToken || user.ResetPasswordTokenExpires < DateTime.UtcNow)
            return new ResetPasswordCommandResponse
            {
                IsSuccess = false,
                Message = "Reset password failed.",
                ValidationsErrors = new Dictionary<string, string>
                {
                    {"Token", "Invalid token."}
                }
            };
        
        var result = await userManager.RemovePasswordAsync(user);
        
        if (!result.Succeeded)
            return new ResetPasswordCommandResponse
            {
                IsSuccess = false,
                Message = "Reset password failed.",
                ValidationsErrors = result.Errors.ToDictionary(x => x.Code, x => x.Description)
            };
        
        result = await userManager.AddPasswordAsync(user, request.ResetPasswordModel.NewPassword!);
        
        if (!result.Succeeded)
            return new ResetPasswordCommandResponse
            {
                IsSuccess = false,
                Message = "Reset password failed.",
                ValidationsErrors = result.Errors.ToDictionary(x => x.Code, x => x.Description)
            };

        return new ResetPasswordCommandResponse
        {
            IsSuccess = true,
            Message = "Reset password succeeded."
        };
    }
}