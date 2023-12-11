using FluentValidation;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Contracts.Commands.DeleteContractById;

public class DeleteContractByIdCommandValidator: AbstractValidator<DeleteContractByIdCommand>
{
    private readonly IContractRepository _contractRepository;
    
    public DeleteContractByIdCommandValidator(IContractRepository contractRepository)
    {
        _contractRepository = contractRepository;
            
        RuleFor(x => x.ContractId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MustAsync(Exists).WithMessage("Contract with Id {PropertyValue} does not exist.");
    }

    private async Task<bool> Exists(Guid arg1, CancellationToken arg2)
    {
        var result = await _contractRepository.FindByIdAsync(arg1);
        return result.IsSuccess;
    }
    
}