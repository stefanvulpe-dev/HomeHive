using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Contracts.Queries.GetAllContractsByOwnerId;

public class GetAllContractsByOwnerIdQueryHandler(IContractRepository repository): 
    IQueryHandler<GetAllContractsByOwnerIdQuery, GetAllContractsByOwnerIdQueryResponse>
{
    public async Task<GetAllContractsByOwnerIdQueryResponse> Handle(GetAllContractsByOwnerIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetContractsByOwnerId(request.OwnerId);
        if (!result.IsSuccess)
            return new GetAllContractsByOwnerIdQueryResponse
            {
                IsSuccess = false,
                Message = $"Contracts not found for owner with id: {request.OwnerId}."
            };

        return new GetAllContractsByOwnerIdQueryResponse
        {
            IsSuccess = true,
            Contracts = result.Value.Select(c => new ContractDto
            {
                Id = c.Id,
                UserId = c.UserId,
                OwnerId = c.OwnerId,
                EstateId = c.EstateId,
                ContractType = c.ContractType.ToString(),
                Status = c.Status.ToString(),
                Price = c.Price,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Description = c.Description
            }).ToList()
        };
    }
    
}