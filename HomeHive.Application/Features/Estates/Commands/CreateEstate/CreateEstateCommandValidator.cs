using FluentValidation;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Estates.Commands.CreateEstate;

public class CreateEstateCommandValidator : AbstractValidator<CreateEstateCommand>
{
    private readonly IUtilityRepository _utilityRepository;
    private readonly IRoomRepository _roomRepository;
    public List<Utility>? Utilities { get; private set; }
    public CreateEstateCommandValidator(IUtilityRepository utilityRepository, IRoomRepository roomRepository)
    {
        _utilityRepository = utilityRepository;
        _roomRepository = roomRepository;
        RuleFor(p => p.EstateData.EstateType)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
        RuleFor(p => p.EstateData.EstateCategory)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
        RuleFor(p => p.EstateData.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("FirstName must not exceed 100 characters.");
        RuleFor(p => p.EstateData.Location)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
        RuleFor(p => p.EstateData.Price)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .GreaterThan(0);
        RuleFor(p => p.EstateData.TotalArea)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
        RuleFor(p => p.EstateData.Utilities)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MustAsync(ValidateUtilitiesExistence!).WithMessage("Utility does not exist.");
        RuleFor(p => p.EstateData.Description)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
        RuleFor(p => p.EstateData.EstateAvatar)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
    }
    private async Task<bool> ValidateUtilitiesExistence(List<string> utilities, CancellationToken cancellationToken)
    {
        var utilitiesResult = await _utilityRepository.GetAllAsync();
        
        Utilities = utilitiesResult.Value.Where(u => utilities.Contains(u.UtilityName!)).ToList();
        
        if (Utilities == null || Utilities.Count != utilities.Count)
            return false;
        
        return true;
    }
}