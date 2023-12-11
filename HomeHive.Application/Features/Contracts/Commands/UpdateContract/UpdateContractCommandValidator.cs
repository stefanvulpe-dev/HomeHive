using FluentValidation;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Contracts.Commands.UpdateContract;

public class UpdateContractCommandValidator : AbstractValidator<UpdateContractCommand>
{
    private readonly IContractRepository _contractRepository;

    public UpdateContractCommandValidator(IContractRepository contractRepository)
    {
        _contractRepository = contractRepository;
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("ContractId is required.");
        RuleFor(p => p.Data)
            .NotNull().WithMessage("ContractData is required.")
            .Custom((contractData, context) =>
            {
                if (string.IsNullOrWhiteSpace(contractData.ContractType!.GetType().Name)
                    && contractData.EstateId == Guid.Empty
                    && contractData.StartDate == null
                    && contractData.EndDate == null
                    && string.IsNullOrWhiteSpace(contractData.Description))
                    context.AddFailure("At least one property of ContractData must be filled.");
            });
    }

    public async Task<Result<Contract>> ValidateContractExistence(UpdateContractCommand command, CancellationToken arg2)
    {
        var contractResult = await _contractRepository.FindByIdAsync(command.Id);
        return contractResult.IsSuccess
            ? Result<Contract>.Success(contractResult.Value)
            : Result<Contract>.Failure("Contract with this id does not exist.");
    }
}