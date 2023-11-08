using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Estates;

namespace HomeHive.Domain.Entities;

public sealed class Estate : BaseEntity
{
    private Estate()
    {
    }

    public Guid OwnerId { get; private set; }
    public User? Owner { get; private set; }
    public EstateType? EstateType { get; private set; }
    public EstateCategory? EstateCategory { get; private set; }
    public string? Name { get; private set; }
    public string? Location { get; private set; }
    public decimal Price { get; private set; }
    public string? TotalArea { get; private set; }
    public string? Utilities { get; private set; }
    public string? Description { get; private set; }
    public string? Image { get; private set; }
    public List<Contract>? Contracts { get; set; }
    public List<Photo>? Photos { get; set; }
    public List<Room>? Rooms { get; set; }

    public static Result<Estate> Create(EstateData estateData)
    {
        var (owner, estateType, estateCategory, name, 
                location, price, totalArea, utilities, 
                description, image) = estateData;
        if (owner == null)
        {
            return Result<Estate>.Failure("Owner is required.");
        }

        if (string.IsNullOrWhiteSpace(estateType) || !Enum.TryParse(estateType, out EstateType typeEnum))
        {
            return Result<Estate>.Failure("EstateType is not valid.");
        }

        if (string.IsNullOrWhiteSpace(estateCategory) ||
            !Enum.TryParse(estateCategory, out EstateCategory categoryEnum))
        {
            return Result<Estate>.Failure("EstateCategory is not valid.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<Estate>.Failure("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(location))
        {
            return Result<Estate>.Failure("Location is required.");
        }

        if (price <= 0)
        {
            return Result<Estate>.Failure("Price is required.");
        }

        if (string.IsNullOrWhiteSpace(totalArea))
        {
            return Result<Estate>.Failure("Total Area is required.");
        }

        if (string.IsNullOrWhiteSpace(utilities))
        {
            return Result<Estate>.Failure("Utilities is required.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            return Result<Estate>.Failure("Description is required.");
        }

        if (string.IsNullOrWhiteSpace(image))
        {
            return Result<Estate>.Failure("Image is required.");
        }

        return Result<Estate>.Success(new Estate
        {
            OwnerId = owner.Id,
            Owner = owner,
            EstateType = Enum.Parse<EstateType>(estateType),
            EstateCategory = Enum.Parse<EstateCategory>(estateCategory),
            Name = name,
            Location = location,
            Price = price,
            TotalArea = totalArea,
            Utilities = utilities,
            Description = description,
            Image = image,
            Contracts = new List<Contract>(),
            Photos = new List<Photo>(),
            Rooms = new List<Room>()
        });
    }
}