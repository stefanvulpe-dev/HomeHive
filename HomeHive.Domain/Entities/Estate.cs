using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Estates;

namespace HomeHive.Domain.Entities;

public sealed class Estate : BaseEntity
{
    private readonly List<Contract>? _contracts = null;
    private readonly List<Photo>? _photos = null;
    private readonly List<Room>? _rooms = null;

    private Estate()
    {
    }

    public Guid OwnerId { get; private set; }
    public EstateType? EstateType { get; private set; }
    public EstateCategory? EstateCategory { get; private set; }
    public string? Name { get; private set; }
    public string? Location { get; private set; }
    public decimal Price { get; private set; }
    public string? TotalArea { get; private set; }
    public string? Utilities { get; private set; }
    public string? Description { get; private set; }
    public string? Image { get; private set; }
    public IReadOnlyList<Contract>? Contracts => _contracts;
    public IReadOnlyList<Photo>? Photos => _photos;
    public IReadOnlyList<Room>? Rooms => _rooms;

    public static Result<Estate> Create(EstateData estateData)
    {
        var (ownerId, estateType, estateCategory, name,
            location, price, totalArea, utilities,
            description, image) = estateData;

        if (string.IsNullOrWhiteSpace(estateType) || !Enum.TryParse(estateType, out EstateType typeEnum))
            return Result<Estate>.Failure("EstateType is not valid.");

        if (string.IsNullOrWhiteSpace(estateCategory) ||
            !Enum.TryParse(estateCategory, out EstateCategory categoryEnum))
            return Result<Estate>.Failure("EstateCategory is not valid.");

        if (string.IsNullOrWhiteSpace(name)) return Result<Estate>.Failure("Name is required.");

        if (string.IsNullOrWhiteSpace(location)) return Result<Estate>.Failure("Location is required.");

        if (price <= 0) return Result<Estate>.Failure("Price is required.");

        if (string.IsNullOrWhiteSpace(totalArea)) return Result<Estate>.Failure("Total Area is required.");

        if (string.IsNullOrWhiteSpace(utilities)) return Result<Estate>.Failure("Utilities is required.");

        if (string.IsNullOrWhiteSpace(description)) return Result<Estate>.Failure("Description is required.");

        if (string.IsNullOrWhiteSpace(image)) return Result<Estate>.Failure("Image is required.");

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
            Image = image
        });
    }

    public void UpdateEstate(EstateData estateData)
    {
        if(estateData.EstateType != null)
        {
            EstateType = Enum.Parse<EstateType>(estateData.EstateType);
        }
        
        if(estateData.EstateCategory != null)
        {
            EstateCategory = Enum.Parse<EstateCategory>(estateData.EstateCategory);
        }
        
        if(estateData.Name != null)
        {
            Name = estateData.Name;
        }
        
        if(estateData.Location != null)
        {
            Location = estateData.Location;
        }
        
        if(estateData.Price != 0)
        {
            Price = estateData.Price;
        }
        
        if(estateData.TotalArea != null)
        {
            TotalArea = estateData.TotalArea;
        }
        
        if(estateData.Utilities != null)
        {
            Utilities = estateData.Utilities;
        }
        
        if(estateData.Description != null)
        {
            Description = estateData.Description;
        }
        
        if(estateData.Image != null)
        {
            Image = estateData.Image;
        }
        
    }
}