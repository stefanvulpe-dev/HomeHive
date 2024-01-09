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
                Id = contract.Id,
                UserId = contract.UserId,
                EstateId = contract.EstateId,
                ContractType = contract.ContractType.ToString(),
                Status = contract.Status.ToString(),
                Price = contract.Price,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                Description = contract.Description
            }).ToList();

        return new GetAllContractsQueryResponse
        {
            IsSuccess = true,
            Contracts = contracts
        };
    }
}