using FluentValidation;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Utilities.Commands.DeleteUtilityById;

public class DeleteUtilityByIdCommandValidator : AbstractValidator<DeleteUtilityByIdCommand>
{
    private readonly IUtilityRepository _utilityRepository;
    public DeleteUtilityByIdCommandValidator(IUtilityRepository utilityRepository)
    {
        _utilityRepository = utilityRepository;
        RuleFor(v => v.UtilityId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} is required.")
            .MustAsync(UtilityExists)
            .WithMessage("A Utility with the specified Id was not found.");
    }
    private async Task<bool> UtilityExists(Guid utilityId, CancellationToken cancellationToken)
    {
        var result = await _utilityRepository.FindByIdAsync(utilityId);
        return result.IsSuccess;
    }
}