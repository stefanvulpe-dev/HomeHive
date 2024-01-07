using FluentValidation;

namespace HomeHive.Application.Features.Estates.Commands.UpdateEstateAvatar;

public class UpdateEstateAvatarCommandValidator: AbstractValidator<UpdateEstateAvatarCommand>
{
    public UpdateEstateAvatarCommandValidator()
    {
        RuleFor(p => p.EstateId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();

        RuleFor(p => p.EstateAvatar)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
    }
}