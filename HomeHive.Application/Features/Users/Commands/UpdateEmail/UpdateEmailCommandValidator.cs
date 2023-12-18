using FluentValidation;
using HomeHive.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeHive.Application.Features.Users.Commands.UpdateEmail;

public class UpdateEmailCommandValidator: AbstractValidator<UpdateEmailCommand>
{
    private readonly UserManager<User> _userManager;
    
    public UpdateEmailCommandValidator(UserManager<User> userManager)
    {
        _userManager = userManager;
        
        RuleFor(v => v.UserId)
            .NotEmpty()
            .WithMessage("UserId is required")
            .MustAsync(ValidateUserExists).WithMessage("User does not exist");
        
        RuleFor(v => v.Model.NewEmail)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid")
            .MustAsync(BeUniqueEmail).WithMessage("The specified email is already in use.");
        
        RuleFor(v => v.Model.EmailResetToken)
            .NotEmpty().WithMessage("Email reset token is required");
    }

    public User? User { get; private set; }
    
    private async Task<bool> ValidateUserExists(Guid arg1, CancellationToken arg2)
    {
        var user = await _userManager.FindByIdAsync(arg1.ToString());
        
        if (user != null)
        {
            User = user;
        }
        
        return user != null;
    }

    private async Task<bool> BeUniqueEmail(string arg1, CancellationToken arg2)
    {
        var user = await _userManager.FindByEmailAsync(arg1);
        return user == null;
    }
}