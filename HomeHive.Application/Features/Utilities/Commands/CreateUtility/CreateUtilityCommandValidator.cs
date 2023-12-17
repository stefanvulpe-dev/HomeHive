using FluentValidation;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Utilities.Commands.CreateUtility;

public class CreateUtilityCommandValidator: AbstractValidator<CreateUtilityCommand>
{
    private readonly IUtilityRepository _utilityRepository;
    public CreateUtilityCommandValidator(IUtilityRepository utilityRepository)
    {
        _utilityRepository = utilityRepository;
        RuleFor(p => p.UtilityName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
            .MustAsync(ValidateUtilityNameDoesNotExist)
            .WithMessage("A utility with the same name already exists.");
    }
    private async Task<bool> ValidateUtilityNameDoesNotExist(string utilityName, CancellationToken cancellationToken)
    {
        var utilitiesResult = await _utilityRepository.GetAllAsync();
        foreach (var utility in utilitiesResult.Value)
        {
            if (utility.UtilityName == utilityName)
                return false;
        }
        return true;
    }
}