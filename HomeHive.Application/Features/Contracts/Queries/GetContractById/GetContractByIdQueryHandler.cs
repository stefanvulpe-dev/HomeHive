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
                UserId = contractResult.Value.UserId,
                EstateId = contractResult.Value.EstateId,
                ContractType = contractResult.Value.ContractType!.GetType().Name,
                StartDate = contractResult.Value.StartDate,
                EndDate = contractResult.Value.EndDate,
                Description = contractResult.Value.Description
            }
        };
    }
}