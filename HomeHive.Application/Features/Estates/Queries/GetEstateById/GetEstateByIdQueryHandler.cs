using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Estates.Queries.GetEstateById;

public class GetEstateByIdQueryHandler(IEstateRepository repository)
    : IQueryHandler<GetEstateByIdQuery, GetEstateByIdResponse>
{
    public async Task<GetEstateByIdResponse> Handle(GetEstateByIdQuery request, CancellationToken cancellationToken)
    {
        var estateResult = await repository.FindByIdAsync(request.Id);

        if (!estateResult.IsSuccess)
            return new GetEstateByIdResponse
            {
                IsSuccess = false,
                Message = "Estate not found."
            };

        return new GetEstateByIdResponse
        {
            IsSuccess = true,
            Estate = new EstateDto
            {
                OwnerId = estateResult.Value.OwnerId,
                EstateType = estateResult.Value.EstateType.ToString(),
                EstateCategory = estateResult.Value.EstateCategory.ToString(),
                Name = estateResult.Value.Name,
                Location = estateResult.Value.Location,
                Price = estateResult.Value.Price,
                TotalArea = estateResult.Value.TotalArea,
                Utilities = estateResult.Value.Utilities,
                Description = estateResult.Value.Description,
                Image = estateResult.Value.Image
            }
        };
    }
}