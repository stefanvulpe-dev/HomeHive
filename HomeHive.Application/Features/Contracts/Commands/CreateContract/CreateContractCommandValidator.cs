using FluentValidation;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Contracts.Commands.CreateContract;

public class CreateContractCommandValidator : AbstractValidator<CreateContractCommand>
{
    private readonly IEstateRepository _estateRepository;

    public CreateContractCommandValidator(IEstateRepository estateRepository)
    {
        _estateRepository = estateRepository;
        RuleFor(p => p.Data.EstateId)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .NotNull()
            .MustAsync(ValidateEstateExistence).WithMessage("Estate does not exist.");
        RuleFor(p => p.Data.OwnerId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull().WithMessage("{PropertyName} can't be null.");
        RuleFor(p => p.Data.Description)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull().WithMessage("{PropertyName} can't be null.")
            .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");
        RuleFor(p => p.Data.StartDate)
            .NotNull().WithMessage("{PropertyName} can't be null.");
        RuleFor(p => p.Data.ContractType)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull().WithMessage("{PropertyName} can't be null.");
        RuleFor(p => p.Data.Status)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull().WithMessage("{PropertyName} can't be null.");
        RuleFor(p => p.Data.Price)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull().WithMessage("{PropertyName} can't be null.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
    }

    private async Task<bool> ValidateEstateExistence(Guid estateId, CancellationToken cancellationToken)
    {
        if (estateId == Guid.Empty) return true;

        var estateResult = await _estateRepository.FindByIdAsync(estateId);

        return estateResult.IsSuccess;
    }
}