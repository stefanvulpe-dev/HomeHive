namespace HomeHive.Application.Features.Estates.Queries;

public record EstateDto(Guid ownerId, string? EstateType, string? EstateCategory, string Name, string Location, decimal Price, string TotalArea, string Utilities, string Description, string Image);