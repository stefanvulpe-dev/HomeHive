using FluentValidation;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Estates.Commands.UpdateEstate;

public class UpdateEstateCommandValidator : AbstractValidator<UpdateEstateCommand>
{
    private readonly IEstateRepository _estateRepository;

    public UpdateEstateCommandValidator(IEstateRepository estateRepository)
    {
        _estateRepository = estateRepository;

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
                    string.IsNullOrWhiteSpace(estateData.Utilities) &&
                    string.IsNullOrWhiteSpace(estateData.Description) &&
                    string.IsNullOrWhiteSpace(estateData.Image))
                    context.AddFailure("At least one field in EstateData must be provided.");
            });
    }

    public async Task<Result<Estate>> EstateExist(UpdateEstateCommand command, CancellationToken arg2)
    {
        var result = await _estateRepository.FindByIdAsync(command.EstateId);
        return result.IsSuccess
            ? Result<Estate>.Success(result.Value)
            : Result<Estate>.Failure("Estate does not exist.");
    }
}