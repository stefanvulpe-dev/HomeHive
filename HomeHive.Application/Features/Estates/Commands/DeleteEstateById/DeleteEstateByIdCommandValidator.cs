using FluentValidation;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Estates.Commands.DeleteEstateById;

public class DeleteEstateByIdCommandValidator : AbstractValidator<DeleteEstateByIdCommand>
{
    private readonly IEstateRepository _estateRepository;
    
    public DeleteEstateByIdCommandValidator(IEstateRepository estateRepository)
    {
        _estateRepository = estateRepository;
        
        RuleFor(v => v.EstateId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MustAsync(EstateExist).WithMessage("Estate does not exist.");
    }
    
    private async Task<bool> EstateExist(Guid arg1, CancellationToken arg2)
    {
        var result = await _estateRepository.FindByIdAsync(arg1);
        return result.IsSuccess;
    }
}