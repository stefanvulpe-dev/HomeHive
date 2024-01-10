using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Estates.Queries.GetEstateById;

public class GetEstateByIdQueryHandler(IEstateRepository repository, IRoomRepository roomRepository, IBlobStorageService blobStorageService, IUserRepository userRepository)
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
        
        var estateAvatarResult = await blobStorageService.GetBlobAsync(estateResult.Value.EstateAvatar!, cancellationToken);
            
        var estateAvatarStream = estateAvatarResult.Value;
        
        var ownerResult = await userRepository.FindByIdAsync(estateResult.Value.OwnerId);
        
        if (!ownerResult.IsSuccess)
            return new GetEstateByIdResponse
            {
                IsSuccess = false,
                Message = "Owner not found."
            };
        
        using var memoryStream = new MemoryStream();
            
        await estateAvatarStream.CopyToAsync(memoryStream, cancellationToken);
            
        var estateAvatarBytes = memoryStream.ToArray();
        
        var estateAvatarBase64 = Convert.ToBase64String(estateAvatarBytes);
        
        var estateAvatar = $"data:image/jpeg;base64,{estateAvatarBase64}";
        
        var estateDto = new EstateDto
        {
            Id = estateResult.Value.Id,
            OwnerId = estateResult.Value.OwnerId,
            OwnerName = ownerResult.Value.FirstName + " " + ownerResult.Value.LastName,
            EstateType = estateResult.Value.EstateType.ToString(),
            EstateCategory = estateResult.Value.EstateCategory.ToString(),
            Name = estateResult.Value.Name,
            Location = estateResult.Value.Location,
            Price = estateResult.Value.Price,
            TotalArea = estateResult.Value.TotalArea,
            Utilities = estateResult.Value.Utilities!.Select(u => u.UtilityType.ToString()).ToList()!,
            EstateRooms = new Dictionary<string, int>(),
            Description = estateResult.Value.Description,
            EstateAvatar = estateAvatar
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