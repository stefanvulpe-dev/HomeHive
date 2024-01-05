using FluentValidation;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Utilities.Commands.UpdateUtility;

public class UpdateUtilityCommandValidator : AbstractValidator<UpdateUtilityCommand>
{
    private readonly IUtilityRepository _utilityRepository;

    public UpdateUtilityCommandValidator(IUtilityRepository utilityRepository)
    {
        _utilityRepository = utilityRepository;

        RuleFor(v => v.UtilityId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();

        RuleFor(v => v.UtilityName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MustAsync(ValidateUtilityNameDoesNotExist)
            .WithMessage("A utility with the same name already exists.");
    }

    public async Task<Result<Utility>> UtilityExists(Guid arg1, CancellationToken arg2)
    {
        var result = await _utilityRepository.FindByIdAsync(arg1);
        return result.IsSuccess
            ? Result<Utility>.Success(result.Value)
            : Result<Utility>.Failure("Utility does not exist.");
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