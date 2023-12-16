using FluentValidation;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Models;

namespace HomeHive.Application.Features.Users.Commands.DeleteProfilePicture;

public class DeleteProfilePictureCommandValidator : AbstractValidator<DeleteProfilePictureCommand>
{
    private readonly IUserRepository _userRepository;

    public DeleteProfilePictureCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required")
            .MustAsync(ValidateProfilePhotoExistence)
            .WithMessage("User has no profile picture set");
    }

    public User? User { get; private set; }

    private async Task<bool> ValidateProfilePhotoExistence(Guid arg1, CancellationToken arg2)
    {
        var userResult = await _userRepository.FindByIdAsync(arg1);

        if (userResult is { IsSuccess: true, Value.ProfilePicture: not null })
        {
            User = userResult.Value;
            return true;
        }

        return false;
    }
}