using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Estates.Queries.GetAllEstates;

public class GetAllEstatesQueryHandler : IQueryHandler<GetAllEstatesQuery, GetAllEstatesResponse>
{
    private readonly IEstateRepository _estateRepository;

    public GetAllEstatesQueryHandler(IEstateRepository estateRepository)
    {
        this._estateRepository = estateRepository;
    }

    public async Task<GetAllEstatesResponse> Handle(GetAllEstatesQuery request, CancellationToken cancellationToken)
    {
        var estates = await _estateRepository.GetAllAsync();

        if (!estates.IsSuccess)
            return new GetAllEstatesResponse
            {
                Success = false,
                Message = "Estates not found.",
                ValidationsErrors = new List<string>() { estates.Error }
            };

        IReadOnlyList<EstateDto>? mappedEstates = estates.Value.Select(estate =>
                new EstateDto(estate.OwnerId, estate.EstateType.ToString(), estate.EstateCategory.ToString(),
                    estate.Name,
                    estate.Location, estate.Price, estate.TotalArea, estate.Utilities, estate.Description,
                    estate.Image))
            .ToList();

        return new GetAllEstatesResponse { Estates = mappedEstates };
    }
}