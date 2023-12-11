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

        var contractResult = await validator.ValidateContractExistence(request, cancellationToken);
        if (!contractResult.IsSuccess)
        {
            return new UpdateContractCommandResponse()
            {
                Success = false,
                Message = $"Contract with {request.Id} does not exist",
                ValidationsErrors = new List<string> { contractResult.Error }
            };
        }

        var newContract = contractResult.Value;
        newContract.Update(request.Data);
        
        await contractRepository.UpdateAsync(newContract);
        return new UpdateContractCommandResponse
        {
            Success = true,
            Contract = new CreateContractDto
            {
                EstateId = newContract.EstateId,
                UserId = newContract.UserId,
                ContractType = newContract.ContractType,
                StartDate = newContract.StartDate,
                EndDate = newContract.EndDate,
                Description = newContract.Description
            }
        };
    }
}
