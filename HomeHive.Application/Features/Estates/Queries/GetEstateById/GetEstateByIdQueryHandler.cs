using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Estates.Queries.GetEstateById;

public class GetEstateByIdQueryHandler : IQueryHandler<GetEstateByIdQuery, GetEstateByIdResponse>
{
    private readonly IEstateRepository _repository;

    public GetEstateByIdQueryHandler(IEstateRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetEstateByIdResponse> Handle(GetEstateByIdQuery request, CancellationToken cancellationToken)
    {
        var estateResult = await _repository.FindByIdAsync(request.Id);

        if (!estateResult.IsSuccess)
            return new GetEstateByIdResponse
            {
                Success = false,
                Message = "Estate not found.",
                ValidationsErrors = new List<string> { estateResult.Error }
            };

        return new GetEstateByIdResponse
        {
            Success = true,
            Estate = new EstateDto(estateResult.Value.OwnerId, estateResult.Value.EstateType.ToString(),
                estateResult.Value.EstateCategory.ToString(), estateResult.Value.Name, estateResult.Value.Location,
                estateResult.Value.Price, estateResult.Value.TotalArea, estateResult.Value.Utilities,
                estateResult.Value.Description, estateResult.Value.Image)
        };
    }
}