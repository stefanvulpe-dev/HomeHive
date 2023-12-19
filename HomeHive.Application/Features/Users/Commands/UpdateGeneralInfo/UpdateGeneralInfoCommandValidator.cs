using FluentValidation;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Models;

namespace HomeHive.Application.Features.Users.Commands.UpdateGeneralInfo;

public class UpdateGeneralInfoCommandValidator: AbstractValidator<UpdateGeneralInfoCommand>
{
    private readonly IUserRepository _userRepository;
    
    public UpdateGeneralInfoCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        
        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustAsync(ValidateUserExists).WithMessage("User does not exist.");
        
        RuleFor(v => v.Data.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .MaximumLength(50).WithMessage("FirstName must not exceed 50 characters.");
        
        RuleFor(v => v.Data.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .MaximumLength(50).WithMessage("LastName must not exceed 50 characters.");
    }

    public User? User { get; private set; }
    
    private async Task<bool> ValidateUserExists(Guid arg1, CancellationToken arg2)
    {
        var user = await _userRepository.FindByIdAsync(arg1);

        if (user.IsSuccess)
        {
            User = user.Value;
        }
        
        return user.IsSuccess;
    }
}