using FluentValidation.Results;
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
        {
            var errors = validationResult.Errors
                .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());
            return new ResetPasswordCommandResponse
            {
                IsSuccess = false,
                Message = "Reset password failed.",
                ValidationsErrors = errors
            };
        }

        var user = validator.User!;
        
        if (user.ResetPasswordToken != request.ResetPasswordModel.ResetPasswordToken || user.ResetPasswordTokenExpires < DateTime.UtcNow)
            return new ResetPasswordCommandResponse
            {
                IsSuccess = false,
                Message = "Reset password failed.",
                ValidationsErrors = new Dictionary<string, List<string>>
                {
                    {"Token", ["Invalid token."] }
                }
            };
        
        var result = await userManager.RemovePasswordAsync(user);
        
        if (!result.Succeeded)
            return new ResetPasswordCommandResponse
            {
                IsSuccess = false,
                Message = "Reset password failed.",
            };
        
        result = await userManager.AddPasswordAsync(user, request.ResetPasswordModel.NewPassword!);
        
        if (!result.Succeeded)
            return new ResetPasswordCommandResponse
            {
                IsSuccess = false,
                Message = "Reset password failed.",
            };

        return new ResetPasswordCommandResponse
        {
            IsSuccess = true,
            Message = "Reset password succeeded."
        };
    }
}