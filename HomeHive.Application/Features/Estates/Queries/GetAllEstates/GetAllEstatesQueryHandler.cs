using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Estates.Queries.GetAllEstates;

public class GetAllEstatesQueryHandler(IEstateRepository estateRepository, IRoomRepository roomRepository, IBlobStorageService blobStorageService)
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

        var mappedEstates = new List<EstateDto>();
        
        foreach(var estate in estates.Value)
        {
            var estateAvatarResult = await blobStorageService.GetBlobAsync(estate.EstateAvatar!, cancellationToken);
            
            var estateAvatarStream = estateAvatarResult.Value;
            
            using var memoryStream = new MemoryStream();
            
            await estateAvatarStream.CopyToAsync(memoryStream, cancellationToken);
            
            var estateAvatarBytes = memoryStream.ToArray();
            var estateAvatarBase64 = Convert.ToBase64String(estateAvatarBytes);
            var estateAvatar = $"data:image/jpeg;base64,{estateAvatarBase64}";
            
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
                Utilities = estate.Utilities!.Select(u => u.UtilityType.ToString()).ToList()!,
                Description = estate.Description,
                EstateAvatar = estateAvatar,
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