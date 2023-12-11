using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Features.Contracts.Commands.CreateContract;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Contracts.Commands.UpdateContract;

public class UpdateContractCommandHandler(IContractRepository contractRepository)
    : ICommandHandler<UpdateContractCommand, UpdateContractCommandResponse>
{
    public async Task<UpdateContractCommandResponse> Handle(UpdateContractCommand request,
        CancellationToken cancellationToken)
    {
        var validator = new UpdateContractCommandValidator(contractRepository);
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validatorResult.IsValid)
            return new UpdateContractCommandResponse
            {
                IsSuccess = false,
                Message = "Failed to update contract.",
                ValidationsErrors = validatorResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage)
            };

        var contractResult = await validator.ValidateContractExistence(request, cancellationToken);
        if (!contractResult.IsSuccess)
            return new UpdateContractCommandResponse
            {
                IsSuccess = false,
                Message = $"Contract with {request.Id} does not exist"
            };

        var newContract = contractResult.Value;
        newContract.Update(request.Data);

        await contractRepository.UpdateAsync(newContract);
        return new UpdateContractCommandResponse
        {
            IsSuccess = true,
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