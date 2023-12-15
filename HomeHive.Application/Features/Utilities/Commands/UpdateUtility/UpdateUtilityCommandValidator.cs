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

        RuleFor(v => v.UtilityData.UtilityName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
    }
    
    public async Task<Result<Utility>> UtilityExists(Guid arg1, CancellationToken arg2)
    {
        var result = await _utilityRepository.FindByIdAsync(arg1);
        return result.IsSuccess
            ? Result<Utility>.Success(result.Value)
            : Result<Utility>.Failure("Utility does not exist.");
    }
}