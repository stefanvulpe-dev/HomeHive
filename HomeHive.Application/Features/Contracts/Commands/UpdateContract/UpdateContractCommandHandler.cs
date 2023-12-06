using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Features.Contracts.Commands.CreateContract;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;

namespace HomeHive.Application.Features.Contracts.Commands.UpdateContract;

public class UpdateContractCommandHandler(IContractRepository contractRepository) : ICommandHandler<UpdateContractCommand, UpdateContractCommandResponse>
{
    public async Task<UpdateContractCommandResponse> Handle(UpdateContractCommand request,
        CancellationToken cancellationToken)
    {

        var validator = new UpdateContractCommandValidator(contractRepository);
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validatorResult.IsValid)
        {
            return new UpdateContractCommandResponse()
            {
                Success = false,
                Message = "Failed to update contract.",
                ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
            };
        }

        var newContract = await contractRepository.FindByIdAsync(request.Id);
        newContract.Value.Update(request.Data);
        
        await contractRepository.UpdateAsync(newContract.Value);
        return new UpdateContractCommandResponse
        {
            Success = true,
            Contract = new CreateContractDto
            {
                Estate = newContract.Value.Estate,
                UserId = newContract.Value.UserId,
                ContractType = newContract.Value.ContractType,
                StartDate = newContract.Value.StartDate,
                EndDate = newContract.Value.EndDate,
                Description = newContract.Value.Description
            }
        };
    }
}
