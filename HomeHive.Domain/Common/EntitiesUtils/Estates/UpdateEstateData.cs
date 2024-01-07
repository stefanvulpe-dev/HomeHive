namespace HomeHive.Domain.Common.EntitiesUtils.Estates;

public record UpdateEstateData(
    string? EstateType,
    string? EstateCategory,
    string? Name,
    string? Location,
    decimal? Price,
    string? TotalArea,
    List<string>? Utilities,
    Dictionary<string, int>? Rooms,
    string? Description
);