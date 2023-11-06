using HomeHive.Domain.Common;

namespace HomeHive.Domain.Entities;

public sealed class Estate: BaseEntity
{
    private Estate()
    {
    }
    
    public User? OwnerId { get; private set; }
    public EstateType? Type { get; private set; }
    public EstateCategory? Category { get; private set; }
    public string? Name { get; private set; }
    public string? Location { get; private set; }
    public decimal Price { get; private set; }
    public string? TotalArea { get; private set; }
    public string? Utilities { get; private set; }
    public string? Description { get; private set; }
    public string? Image { get; private set; }
    
    public static Result<Estate> Create(User? ownerId, EstateType? type, EstateCategory? category, string name,
        string location, decimal price, string totalArea, string utilities, string description, string image)
    {
        if (ownerId == null)
        {
            return Result<Estate>.Failure("Owner Id is required.");
        }
        
        if (type == null)
        {
            return Result<Estate>.Failure("Type is required.");
        }

        if (category == null)
        {
            return Result<Estate>.Failure("Category is required.");
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
            OwnerId = ownerId,
            Type = type,
            Category = category,
            Name = name,
            Location = location,
            Price = price,
            TotalArea = totalArea,
            Utilities = utilities,
            Description = description,
            Image = image
        });
    }
    public enum EstateType
    {
        House,
        Apartment,
        Villa,
        Cottage,
        Farmhouse,
        Bungalow,
        Townhouse,
        Penthouse,
        Studio,
        Duplex,
        Flat,
    }
    
    public enum EstateCategory
    {
        ForRent, ForSale
    }
}