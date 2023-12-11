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
    }

    private async Task<bool> ValidateEstateExistence(Guid estateId, CancellationToken cancellationToken)
    {
        var estateResult = await _estateRepository.FindByIdAsync(estateId);
        Console.WriteLine(estateResult.IsSuccess);
        return estateResult.IsSuccess;
    }
}