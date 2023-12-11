using FluentValidation;

namespace HomeHive.Application.Features.Estates.Commands.CreateEstate;

public class CreateEstateCommandValidator : AbstractValidator<CreateEstateCommand>
{
    public CreateEstateCommandValidator()
    {
        RuleFor(p => p.EstateData.EstateType)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
        RuleFor(p => p.EstateData.EstateCategory)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
        RuleFor(p => p.EstateData.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("FirstName must not exceed 100 characters.");
        RuleFor(p => p.EstateData.Location)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
        RuleFor(p => p.EstateData.Price)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .GreaterThan(0);
        RuleFor(p => p.EstateData.TotalArea)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
        RuleFor(p => p.EstateData.Utilities)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
        RuleFor(p => p.EstateData.Description)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
        RuleFor(p => p.EstateData.Image)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
    }
}