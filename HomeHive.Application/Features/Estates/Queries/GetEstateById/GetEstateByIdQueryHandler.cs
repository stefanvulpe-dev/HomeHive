using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Estates.Queries.GetEstateById;

public class GetEstateByIdQueryHandler(IEstateRepository repository, IRoomRepository roomRepository)
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

        var estateDto = new EstateDto
        {
            OwnerId = estateResult.Value.OwnerId,
            EstateType = estateResult.Value.EstateType.ToString(),
            EstateCategory = estateResult.Value.EstateCategory.ToString(),
            Name = estateResult.Value.Name,
            Location = estateResult.Value.Location,
            Price = estateResult.Value.Price,
            TotalArea = estateResult.Value.TotalArea,
            Utilities = estateResult.Value.Utilities.Select(u => u.UtilityName).ToList(),
            EstateRooms = new Dictionary<string, int>(),
            Description = estateResult.Value.Description,
            EstateAvatar = estateResult.Value.EstateAvatar
        };

        foreach (var estateRoom in estateResult.Value.EstateRooms!)
        {
            var roomResult = await roomRepository.FindByIdAsync(estateRoom.RoomId);
            
            if (roomResult.IsSuccess)
            {
                estateDto.EstateRooms[roomResult.Value.RoomType.ToString()!] = estateRoom.Quantity;
            }
        }

        return new GetEstateByIdResponse
        {
            IsSuccess = true,
            Estate = estateDto
        };
    }
}