using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Contracts.Commands.DeleteContractById;

public class DeleteContractByIdCommandHandler: ICommandHandler<DeleteContractByIdCommand, DeleteContractByIdCommandResponse>
{
    private readonly IContractRepository _contractRepository;

    public DeleteContractByIdCommandHandler(IContractRepository contractRepository)
    {
        _contractRepository = contractRepository;
    }

    public async Task<DeleteContractByIdCommandResponse> Handle(DeleteContractByIdCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteContractByIdCommandValidator(_contractRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return new DeleteContractByIdCommandResponse
            {
                Success = false,
                ValidationsErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            };
        
        var result = await _contractRepository.DeleteByIdAsync(request.ContractId);
        if (!result.IsSuccess)
            return new DeleteContractByIdCommandResponse
            {
                Success = false,
                Message = $"Error deleting contract with Id {request.ContractId}"
            };

        return new DeleteContractByIdCommandResponse
        {
            Message = $"Contract with Id {request.ContractId} deleted"
        };
    }
}