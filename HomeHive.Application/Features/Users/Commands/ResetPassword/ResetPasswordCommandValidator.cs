using FluentValidation;
using HomeHive.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeHive.Application.Features.Users.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPassword.ResetPasswordCommand>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordCommandValidator(UserManager<User> userManager)
    {
        _userManager = userManager;
        RuleFor(p => p.UserId)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .MustAsync(ValidateUserExistence)
            .WithMessage("User does not exist.");

        RuleFor(p => p.ResetPasswordModel.ResetPasswordToken)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.");
       
        RuleFor(x => x.ResetPasswordModel.NewPassword)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-=]).{8,}$")
            .WithMessage(
                "{PropertyName} must contain at least 8 characters, one uppercase, one lowercase, one number and one special character: #?!@$ %^&*-")
            .MaximumLength(25)
            .WithMessage("{PropertyName} must not exceed 25 characters");
    }

    public User? User { get; private set; }
    
    private async Task<bool> ValidateUserExistence(Guid userId, CancellationToken arg2)
    {
        var userResult = await _userManager.FindByIdAsync(userId.ToString());

        if (userResult == null) return false;

        User = userResult;

        return true;
    }
}