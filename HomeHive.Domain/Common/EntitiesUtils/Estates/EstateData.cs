namespace HomeHive.Domain.Common.EntitiesUtils.Estates;

public record EstateData(
    string? EstateType,
    string? EstateCategory,
    string? Name,
    string? Location,
    decimal? Price,
    string? TotalArea,
    string? Utilities,
    string? Description,
    string? Image);