using FluentValidation;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Estates.Commands.UpdateEstatePhotos;

public class UpdateEstatePhotosCommandValidator: AbstractValidator<UpdateEstatePhotosCommand>
{
    private readonly IEstateRepository _estateRepository;
    
    public UpdateEstatePhotosCommandValidator(IEstateRepository estateRepository)
    {
        _estateRepository = estateRepository;
        RuleFor(command => command.EstateId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MustAsync(ValidateEstateExistence).WithMessage("Estate with id {PropertyValue} not found.");

        RuleFor(command => command.EstatePhotos)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .Must(photos => photos.Count >= 5).WithMessage("{PropertyName} must have minimum 5 items.")
            .Must(photos => photos.Count <= 10).WithMessage("{PropertyName} must have maximum 10 items.");
    }

    public Estate? Estate { get; private set; }
    
    private async Task<bool> ValidateEstateExistence(Guid arg1, CancellationToken arg2)
    {
        var estateResult = await _estateRepository.FindByIdAsync(arg1);
        if (estateResult.IsSuccess)
            Estate = estateResult.Value;
        return estateResult.IsSuccess;
    }
}