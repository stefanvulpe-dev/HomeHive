using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Contracts.Commands.CreateContract;

public class CreateContractCommandHandler(IContractRepository contractRepository) : ICommandHandler<CreateContractCommand, CreateContractCommandResponse>
{
    public async Task<CreateContractCommandResponse> Handle(CreateContractCommand request, CancellationToken cancellationToken)
    {
        
        var validator = new CreateContractCommandValidator();
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (!validatorResult.IsValid)
        {
            return new CreateContractCommandResponse()
            {
                Success = false,
                Message = "Failed to create contract.",
                ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
            };
        }
        
        var contract = Contract.Create(request.UserId, request.Data);
        if (!contract.IsSuccess)
        {
            return new CreateContractCommandResponse
            {
                Success = false,
                ValidationsErrors = new List<string> { contract.Error }
            };
        }
       
        await contractRepository.AddAsync(contract.Value);
        return new CreateContractCommandResponse
        {
            Success = true,
            Contract = new CreateContractDto
            {
                ContractId = contract.Value.Id, 
                EstateId = contract.Value.EstateId,
                UserId = contract.Value.UserId,
                ContractType = contract.Value.ContractType,
                StartDate = contract.Value.StartDate,
                EndDate = contract.Value.EndDate,
                Description = contract.Value.Description
            }
        };
    }
}