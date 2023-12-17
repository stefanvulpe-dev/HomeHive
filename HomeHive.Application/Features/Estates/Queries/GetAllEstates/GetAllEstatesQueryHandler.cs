using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Estates.Queries.GetAllEstates;

public class GetAllEstatesQueryHandler(IEstateRepository estateRepository)
    : IQueryHandler<GetAllEstatesQuery, GetAllEstatesResponse>
{
    public async Task<GetAllEstatesResponse> Handle(GetAllEstatesQuery request, CancellationToken cancellationToken)
    {
        var estates = await estateRepository.GetAllAsync();

        if (!estates.IsSuccess)
            return new GetAllEstatesResponse
            {
                IsSuccess = false,
                Message = "Estates not found."
            };
        
        IReadOnlyList<EstateDto>? mappedEstates = estates.Value.Select(estate =>
                new EstateDto
                {
                    OwnerId = estate.OwnerId,
                    EstateType = estate.EstateType.ToString(),
                    EstateCategory = estate.EstateCategory.ToString(),
                    Name = estate.Name,
                    Location = estate.Location,
                    Price = estate.Price,
                    TotalArea = estate.TotalArea,
                    Utilities = estate.Utilities.Select(u => u.UtilityName).ToList(),
                    Description = estate.Description,
                    EstateAvatar = estate.EstateAvatar
                })
            .ToList();

        return new GetAllEstatesResponse { Estates = mappedEstates };
    }
}