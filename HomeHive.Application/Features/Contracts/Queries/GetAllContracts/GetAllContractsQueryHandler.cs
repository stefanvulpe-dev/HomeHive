using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Contracts.Queries.GetAllContracts;

public class GetAllContractsQueryHandler(IContractRepository repository)
    : IQueryHandler<GetAllContractsQuery, GetAllContractsQueryResponse>
{
    public async Task<GetAllContractsQueryResponse> Handle(GetAllContractsQuery request,
        CancellationToken cancellationToken)
    {
        var contractsResult = await repository.GetAllAsync();

        if (!contractsResult.IsSuccess)
            return new GetAllContractsQueryResponse
            {
                IsSuccess = false,
                Message = "Contracts not found."
            };

        IReadOnlyList<ContractDto> contracts = contractsResult.Value.Select(contract =>
            new ContractDto
            {
                UserId = contract.UserId,
                EstateId = contract.EstateId,
                ContractType = contract.ContractType!.GetType().Name,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                Description = contract.Description
            }).ToList();

        return new GetAllContractsQueryResponse { Contracts = contracts };
    }
}