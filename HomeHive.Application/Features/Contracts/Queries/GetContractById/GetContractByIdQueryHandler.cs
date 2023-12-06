using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Contracts.Queries.GetContractById;

public class GetContractByIdQueryHandler(IContractRepository repository): IQueryHandler<GetContractByIdQuery, GetContractByIdResponse>
{
    public async Task<GetContractByIdResponse> Handle(GetContractByIdQuery request, CancellationToken cancellationToken)
    {
        var contractResult = await repository.FindByIdAsync(request.Id);

        if (!contractResult.IsSuccess)
        {
            return new GetContractByIdResponse
            {
                Success = false,
                Message = "Contract not found.",
                ValidationsErrors = new List<string> { contractResult.Error }
            };
        }

        return new GetContractByIdResponse
        {
            Success = true,
            Contract = new ContractDto(contractResult.Value.UserId, contractResult.Value.EstateId,
                contractResult.Value.ContractType!.GetType().Name,
                contractResult.Value.StartDate, contractResult.Value.EndDate, contractResult.Value.Description)
        };
    }
}