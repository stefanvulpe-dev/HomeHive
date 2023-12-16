using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Contracts.Queries.GetAllContractsByUserId;

public class GetAllContractsByUserIdQueryHandler(IContractRepository repository)
    : IQueryHandler<GetAllContractsByUserIdQuery, GetAllContractsByUserIdResponse>
{
    public async Task<GetAllContractsByUserIdResponse> Handle(GetAllContractsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetContractsByUserId(request.UserId);
        if (!result.IsSuccess)
            return new GetAllContractsByUserIdResponse
            {
                IsSuccess = false,
                Message = $"Contracts not found for user with id: {request.UserId}"
            };

        return new GetAllContractsByUserIdResponse
        {
            IsSuccess = true,
            Contracts = result.Value.Select(c => new UserContractDto
            {
                EstateId = c.EstateId,
                ContractType = c.ContractType!.Value.ToString(),
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Description = c.Description
            }).ToList()
        };
    }
}