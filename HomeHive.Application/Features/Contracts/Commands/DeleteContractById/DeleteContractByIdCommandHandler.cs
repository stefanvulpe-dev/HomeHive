using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Contracts.Commands.DeleteContractById;

public class DeleteContractByIdCommandHandler(IContractRepository contractRepository)
    : ICommandHandler<DeleteContractByIdCommand, DeleteContractByIdCommandResponse>
{
    public async Task<DeleteContractByIdCommandResponse> Handle(DeleteContractByIdCommand request,
        CancellationToken cancellationToken)
    {
        var validator = new DeleteContractByIdCommandValidator(contractRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return new DeleteContractByIdCommandResponse
            {
                IsSuccess = false,
                ValidationsErrors = validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage)
            };

        var result = await contractRepository.DeleteByIdAsync(request.ContractId);
        if (!result.IsSuccess)
            return new DeleteContractByIdCommandResponse
            {
                IsSuccess = false,
                Message = $"Error deleting contract with Id {request.ContractId}"
            };

        return new DeleteContractByIdCommandResponse
        {
            Message = $"Contract with Id {request.ContractId} deleted"
        };
    }
}