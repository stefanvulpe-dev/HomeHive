using FluentValidation;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Estates.Commands.UpdateEstate;

public class UpdateEstateCommandValidator : AbstractValidator<UpdateEstateCommand>
{
    private readonly IEstateRepository _estateRepository;
    private readonly IUtilityRepository _utilityRepository;

    public UpdateEstateCommandValidator(IEstateRepository estateRepository, IUtilityRepository utilityRepository)
    {
        _estateRepository = estateRepository;
        _utilityRepository = utilityRepository;

        RuleFor(v => v.EstateId)
            .NotEmpty().WithMessage("EstateId is required.");
        RuleFor(p => p.EstateData)
            .NotNull().WithMessage("EstateData is required.")
            .Custom((estateData, context) =>
            {
                if (string.IsNullOrWhiteSpace(estateData.EstateType) &&
                    string.IsNullOrWhiteSpace(estateData.EstateCategory) &&
                    string.IsNullOrWhiteSpace(estateData.Name) &&
                    string.IsNullOrWhiteSpace(estateData.Location) &&
                    estateData.Price <= 0 &&
                    string.IsNullOrWhiteSpace(estateData.TotalArea) &&
                    (estateData.Utilities == null || estateData.Utilities.Count == 0) &&
                    string.IsNullOrWhiteSpace(estateData.Description) &&
                    string.IsNullOrWhiteSpace(estateData.EstateAvatar))
                    context.AddFailure("At least one field in EstateData must be provided.");
            });
        RuleFor(v => v.EstateData.Utilities)
            .MustAsync(ValidateUtilitiesExistence).WithMessage("Utility does not exist.")
            .When(v => v.EstateData.Utilities != null && v.EstateData.Utilities.Count > 0);
    }

    public List<Utility>? Utilities { get; private set; } = new();

    public async Task<Result<Estate>> EstateExist(UpdateEstateCommand command, CancellationToken arg2)
    {
        var result = await _estateRepository.FindByIdAsync(command.EstateId);
        return result.IsSuccess
            ? Result<Estate>.Success(result.Value)
            : Result<Estate>.Failure("Estate does not exist.");
    }

    private async Task<bool> ValidateUtilitiesExistence(List<string>? utilities, CancellationToken cancellationToken)
    {
        var utilitiesResult = await _utilityRepository.GetAllAsync();
        var utilitiesNames = utilitiesResult.Value.Select(u => u.UtilityName).ToList();
        if (utilities!.Any(utility => !utilitiesNames.Contains(utility))) return false;
        Utilities = utilitiesResult.Value.Where(u => utilities!.Contains(u.UtilityName!)).ToList();
        return true;
    }
}