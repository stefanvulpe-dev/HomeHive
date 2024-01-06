using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Estates;

namespace HomeHive.Domain.Entities;

public sealed class Estate : BaseEntity
{
    private readonly List<Contract>? _contracts = null;
    private readonly List<EstatePhoto>? _estatePhotos = null;

    private Estate()
    {
    }

    public Guid OwnerId { get; private set; }
    public EstateType? EstateType { get; private set; }
    public EstateCategory? EstateCategory { get; private set; }
    public string? Name { get; private set; }
    public string? Location { get; private set; }
    public decimal? Price { get; private set; }
    public string? TotalArea { get; private set; }
    public List<Utility>? Utilities { get; private set; }
    public List<EstateRoom>? EstateRooms { get; private set; }
    public string? Description { get; private set; }
    public string? EstateAvatar { get; private set; }
    public IReadOnlyList<Contract>? Contracts => _contracts;
    public IReadOnlyList<EstatePhoto>? EstatePhotos => _estatePhotos;

    public static Result<Estate> Create(Guid ownerId, List<Utility> utilities, CreateEstateData createEstateData)
    {
        var (estateType, estateCategory, name,
            location, price, totalArea, utilitiesNames, 
            rooms , description, image) = createEstateData;

        if (ownerId == Guid.Empty) return Result<Estate>.Failure("OwnerId is required.");

        if (string.IsNullOrWhiteSpace(estateType) || !Enum.TryParse(estateType, out EstateType typeEnum))
            return Result<Estate>.Failure("EstateType is not valid.");

        if (string.IsNullOrWhiteSpace(estateCategory) ||
            !Enum.TryParse(estateCategory, out EstateCategory categoryEnum))
            return Result<Estate>.Failure("EstateCategory is not valid.");

        if (string.IsNullOrWhiteSpace(name)) return Result<Estate>.Failure("Name is required.");

        if (string.IsNullOrWhiteSpace(location)) return Result<Estate>.Failure("Location is required.");

        if (price <= 0) return Result<Estate>.Failure("Price is required.");

        if (string.IsNullOrWhiteSpace(totalArea)) return Result<Estate>.Failure("Total Area is required.");
        
        if (utilitiesNames == null || utilitiesNames.Count == 0) 
            return Result<Estate>.Failure("Utilities are required.");
        
        foreach (var utilityName in utilitiesNames)
        {
                if (string.IsNullOrWhiteSpace(utilityName)) return Result<Estate>.Failure("Utility name is required.");
        }
        
        if (rooms == null || rooms.Count == 0) return Result<Estate>.Failure("Rooms are required.");
        
        foreach (var pair in rooms)
        {
            if (string.IsNullOrWhiteSpace(pair.Key)) return Result<Estate>.Failure("Room type is required.");
            
            if (pair.Value <= 0) return Result<Estate>.Failure("Room quantity is required.");
        }
    
        if (string.IsNullOrWhiteSpace(description)) return Result<Estate>.Failure("Description is required.");

        if (string.IsNullOrWhiteSpace(image)) return Result<Estate>.Failure("Estate Avatar is required.");

        return Result<Estate>.Success(new Estate
        {
            OwnerId = ownerId,
            EstateType = Enum.Parse<EstateType>(estateType),
            EstateCategory = Enum.Parse<EstateCategory>(estateCategory),
            Name = name,
            Location = location,
            Price = price,
            TotalArea = totalArea,
            Utilities = utilities,
            Description = description,
            EstateAvatar = image
        });
    }

    public void Update(List<Utility> utilities, List<EstateRoom> rooms, CreateEstateData createEstateData)
    {
        if (createEstateData.EstateType != null) EstateType = Enum.Parse<EstateType>(createEstateData.EstateType);

        if (createEstateData.EstateCategory != null) EstateCategory = Enum.Parse<EstateCategory>(createEstateData.EstateCategory);

        if (createEstateData.Name != null) Name = createEstateData.Name;

        if (createEstateData.Location != null) Location = createEstateData.Location;

        if (createEstateData.Price != null) Price = createEstateData.Price;

        if (createEstateData.TotalArea != null) TotalArea = createEstateData.TotalArea;
        
        if (createEstateData.Utilities != null) Utilities = utilities;
        
        if (createEstateData.Rooms != null) EstateRooms = rooms;

        if (createEstateData.Description != null) Description = createEstateData.Description;

        if (createEstateData.EstateAvatar != null) EstateAvatar = createEstateData.EstateAvatar;
    }
    
    public void AddEstateRooms(List<EstateRoom> estateRooms)
    {
        EstateRooms = estateRooms;
    }
}