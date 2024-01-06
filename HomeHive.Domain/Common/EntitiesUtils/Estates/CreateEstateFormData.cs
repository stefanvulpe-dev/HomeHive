using Microsoft.AspNetCore.Http;

namespace HomeHive.Domain.Common.EntitiesUtils.Estates;

public record EstateFormData(
    string? EstateType,
    string? EstateCategory,
    string? Name,
    string? Location,
    decimal? Price,
    string? TotalArea,
    string? Description,
    IFormFile? EstateAvatar
);

public record CreateEstateFormData(
    string? EstateType,
    string? EstateCategory,
    string? Name,
    string? Location,
    decimal? Price,
    string? TotalArea,
    List<string>? Utilities,
    Dictionary<string, int>? Rooms,
    string? Description,
    IFormFile? EstateAvatar
);