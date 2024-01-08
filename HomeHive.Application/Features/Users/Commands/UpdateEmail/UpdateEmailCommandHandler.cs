using System.Text.RegularExpressions;
using HomeHive.Application.Contracts.Commands;
using HomeHive.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeHive.Application.Features.Users.Commands.UpdateEmail;

public class UpdateEmailCommandHandler(UserManager<User> userManager) : ICommandHandler<UpdateEmailCommand, UpdateEmailCommandResponse>
{
    public async Task<UpdateEmailCommandResponse> Handle(UpdateEmailCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateEmailCommandValidator(userManager);
        var validatorResult = await validator.ValidateAsync(command, cancellationToken);
        
        if (!validatorResult.IsValid)
        {
            var validationErrors = validatorResult.Errors
                .GroupBy(x => Regex.Replace(x.PropertyName, ".*\\.", ""), x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());
            return new UpdateEmailCommandResponse
            {
                IsSuccess = false,
                Message = "Update email validation failed",
                ValidationsErrors = validationErrors
            };
        }
        
        var user = validator.User!;
        
        if (user.ResetEmailToken != command.Model.EmailResetToken || user.ResetEmailTokenExpires < DateTime.UtcNow)
            return new UpdateEmailCommandResponse
            {
                IsSuccess = false,
                Message = "Update email failed.",
                ValidationsErrors = new Dictionary<string, List<string>>
                {
                    {"Token", ["Invalid token."] }
                }
            };
        
        var result = await userManager.SetEmailAsync(user, command.Model.NewEmail!);
        
        if (!result.Succeeded)
            return new UpdateEmailCommandResponse
            {
                IsSuccess = false,
                Message = "Update email failed."
            };

        return new UpdateEmailCommandResponse
        {
            IsSuccess = true,
            Message = "Update email succeeded."
        };
    }
}