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
        
        RuleSet("UpdateEstate", () =>
        {
            RuleFor(v => v.EstateId)
                .NotEmpty().WithMessage("EstateId is required.");
            RuleFor(p => p.EstateData)
                .NotNull().WithMessage("EstateData is required.")
                .Custom((estatedata, context) =>
                {
                    if (string.IsNullOrWhiteSpace(estatedata.EstateType) &&
                        string.IsNullOrWhiteSpace(estatedata.EstateCategory) &&
                        string.IsNullOrWhiteSpace(estatedata.Name) &&
                        string.IsNullOrWhiteSpace(estatedata.Location) &&
                        (estatedata.Price <= 0) &&
                        string.IsNullOrWhiteSpace(estatedata.TotalArea) &&
                        string.IsNullOrWhiteSpace(estatedata.Utilities) &&
                        string.IsNullOrWhiteSpace(estatedata.Description) &&
                        string.IsNullOrWhiteSpace(estatedata.Image))
                    {
                        context.AddFailure("At least one field in EstateData must be provided.");
                    }
                });
        });
    }
    
    public async Task<Result<Estate>> EstateExist(UpdateEstateCommand command, CancellationToken arg2)
    {
        var result = await _estateRepository.FindByIdAsync(command.EstateId);
        return result.IsSuccess ? Result<Estate>.Success(result.Value) : Result<Estate>.Failure("Estate does not exist.");
    }
}