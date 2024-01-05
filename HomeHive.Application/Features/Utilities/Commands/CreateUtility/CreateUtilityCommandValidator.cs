using FluentValidation;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Utilities.Commands.CreateUtility;

public class CreateUtilityCommandValidator : AbstractValidator<CreateUtilityCommand>
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
        if (string.IsNullOrEmpty(utilityName))
            return true;

        var utilitiesResult = await _utilityRepository.GetAllAsync();
        if (!utilitiesResult.IsSuccess)
            return false;
        
        var utilities = utilitiesResult.Value.Select(u => u.UtilityType.ToString()).ToList();
        return !utilities.Contains(utilityName);
    }
}