using FluentValidation;

namespace HomeHive.Application.Features.Commands.CreateUser;

public class CreateUserCommandValidator: AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {   
        RuleSet("CreateUser", () =>
        {
            RuleFor(p => p.UserData.FirstName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("FirstName must not exceed 100 characters.");
            RuleFor(p => p.UserData.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("LastName must not exceed 50 characters.");
            RuleFor(p => p.UserData.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");
            RuleFor(p => p.UserData.Password)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");
            RuleFor(p => p.UserData.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(20).WithMessage("PhoneNumber must not exceed 20 characters.");
            RuleFor(p => p.UserData.ProfilePicture)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("ProfilePicture must not exceed 100 characters.");
        });
    }
}
