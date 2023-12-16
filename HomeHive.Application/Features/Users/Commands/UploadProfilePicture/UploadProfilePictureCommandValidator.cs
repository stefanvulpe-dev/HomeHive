using FluentValidation;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Models;

namespace HomeHive.Application.Features.Users.Commands.UploadProfilePicture;

public class UploadProfilePictureCommandValidator : AbstractValidator<UploadProfilePictureCommand>
{
    private readonly IUserRepository _userRepository;

    public UploadProfilePictureCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        var allowedContentTypes = new[] { "image/jpeg", "image/png" };

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required")
            .MustAsync(ValidateUserExistence)
            .WithMessage("User does not exist");

        RuleFor(x => x.Content)
            .NotNull()
            .WithMessage("Profile picture is required");

        RuleFor(x => x.ContentType)
            .NotNull()
            .WithMessage("ContentType is required")
            .Must(allowedContentTypes.Contains)
            .WithMessage("{propertyName} must be one of the following types: {allowedContentTypes}");
    }

    public User? User { get; private set; }

    private async Task<bool> ValidateUserExistence(Guid userId, CancellationToken arg2)
    {
        var userResult = await _userRepository.FindByIdAsync(userId);

        if (!userResult.IsSuccess) return false;

        User = userResult.Value;

        return true;
    }
}