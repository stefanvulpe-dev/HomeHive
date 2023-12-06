using FluentValidation;

namespace HomeHive.Application.Features.Contracts.Commands.CreateContract;

public class CreateContractCommandValidator: AbstractValidator<CreateContractCommand>
{
    public CreateContractCommandValidator()
    {
        RuleSet("CreateContract", () =>
        {
            RuleFor(p => p.Data.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");
            RuleFor(p => p.Data.StartDate)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.Data.EndDate)
                .NotEmpty().WithMessage("{ProprietyName} is required.")
                .NotNull();
            RuleFor(p => p.Data.ContractType)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        });
    }
}