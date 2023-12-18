using FluentValidation;
using HomeHive.Application.Models;

namespace HomeHive.Identity.Services.Authentication;

public sealed class UserRegistrationValidator : AbstractValidator<RegistrationModel>
{
    public UserRegistrationValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .EmailAddress()
            .WithMessage("{PropertyName} is not a valid email address");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .MinimumLength(5)
            .WithMessage("{PropertyName} must be at least 5 characters long")
            .MaximumLength(25)
            .WithMessage("{PropertyName} must not exceed 25 characters");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .MinimumLength(3)
            .WithMessage("{PropertyName} must be at least 3 characters long")
            .MaximumLength(25)
            .WithMessage("{PropertyName} must not exceed 25 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .MinimumLength(3)
            .WithMessage("{PropertyName} must be at least 3 characters long")
            .MaximumLength(25)
            .WithMessage("{PropertyName} must not exceed 25 characters");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$")
            .WithMessage(
                "{PropertyName} must contain at least 8 characters, one uppercase, one lowercase, one number and one special character: #?!@$ %^&*-")
            .MaximumLength(25)
            .WithMessage("{PropertyName} must not exceed 25 characters");
    }
}