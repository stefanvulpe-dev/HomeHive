using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Contracts.Queries.GetAllContracts;

public class GetAllContractsQueryHandler(IContractRepository repository): IQueryHandler<GetAllContractsQuery, GetAllContractsQueryResponse>
{
    public async Task<GetAllContractsQueryResponse> Handle(GetAllContractsQuery request, CancellationToken cancellationToken)
    {
        var contractsResult = await repository.GetAllAsync();
        
        if (!contractsResult.IsSuccess)
            return new GetAllContractsQueryResponse
            {
                Success = false,
                Message = "Contracts not found."
            };
        
        IReadOnlyList<ContractDto> contracts = contractsResult.Value.Select(contract =>
            new ContractDto(contract.UserId, contract.EstateId, contract.ContractType.GetType().Name, contract.StartDate, contract.EndDate, contract.Description)).ToList();

        return new GetAllContractsQueryResponse { Contracts = contracts };
    }
}