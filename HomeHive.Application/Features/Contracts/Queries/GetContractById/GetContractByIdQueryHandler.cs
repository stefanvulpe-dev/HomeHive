using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Contracts.Queries.GetContractById;

public class GetContractByIdQueryHandler(IContractRepository repository)
    : IQueryHandler<GetContractByIdQuery, GetContractByIdResponse>
{
    public async Task<GetContractByIdResponse> Handle(GetContractByIdQuery request, CancellationToken cancellationToken)
    {
        var contractResult = await repository.FindByIdAsync(request.Id);

        if (!contractResult.IsSuccess)
            return new GetContractByIdResponse
            {
                IsSuccess = false,
                Message = "Contract not found."
            };

        return new GetContractByIdResponse
        {
            IsSuccess = true,
            Contract = new ContractDto
            {
                Id = contractResult.Value.Id,
                UserId = contractResult.Value.UserId,
                EstateId = contractResult.Value.EstateId,
                ContractType = contractResult.Value.ContractType.ToString(),
                Status = contractResult.Value.Status.ToString(),
                Price = contractResult.Value.Price,
                StartDate = contractResult.Value.StartDate,
                EndDate = contractResult.Value.EndDate,
                Description = contractResult.Value.Description
            }
        };
    }
}