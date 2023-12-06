using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Estates.Queries.GetAllEstates;

public class GetAllEstatesQueryHandler : IQueryHandler<GetAllEstatesQuery, GetAllEstatesResponse>
{
    private readonly IEstateRepository estateRepository;
    
    public GetAllEstatesQueryHandler(IEstateRepository estateRepository)
    {
        this.estateRepository = estateRepository;
    }
    
    public async Task<GetAllEstatesResponse> Handle(GetAllEstatesQuery request, CancellationToken cancellationToken)
    {
        var estates = await estateRepository.GetAllAsync();
        
        if (!estates.IsSuccess)
            return new GetAllEstatesResponse
            {
                Success = false,
                Message = "Estates not found."
            };
        
        IReadOnlyList<EstateDto>? mappedEstates = estates.Value.Select(estate =>
            new EstateDto(estate.OwnerId, estate.EstateType.ToString(), estate.EstateCategory.ToString(), estate.Name, estate.Location, estate.Price, estate.TotalArea, estate.Utilities, estate.Description, estate.Image)).ToList();
        
        return new GetAllEstatesResponse { Estates = mappedEstates };
    }
}