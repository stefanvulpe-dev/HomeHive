using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Estates.Queries.GetAllEstates;

public class GetAllEstatesQueryHandler(IEstateRepository estateRepository, IRoomRepository roomRepository)
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

        List<EstateDto>? mappedEstates = new List<EstateDto>();
        foreach(var estate in estates.Value)
        {
            var estateDto = new EstateDto
            {
                Id = estate.Id,
                OwnerId = estate.OwnerId,
                EstateType = estate.EstateType.ToString(),
                EstateCategory = estate.EstateCategory.ToString(),
                Name = estate.Name,
                Location = estate.Location,
                Price = estate.Price,
                TotalArea = estate.TotalArea,
                Utilities = estate.Utilities!.Select(u => u.UtilityType.ToString()).ToList(),
                Description = estate.Description,
                EstateAvatar = estate.EstateAvatar,
                EstateRooms = new Dictionary<string, int>()
            };

            foreach (var estateRoom in estate.EstateRooms!)
            {
                var roomResult = await roomRepository.FindByIdAsync(estateRoom.RoomId);

                if (roomResult.IsSuccess)
                {
                    estateDto.EstateRooms[roomResult.Value.RoomType.ToString()!] = estateRoom.Quantity;
                }
            }
            mappedEstates.Add(estateDto);
        }

        return new GetAllEstatesResponse { Estates = mappedEstates };
    }
}