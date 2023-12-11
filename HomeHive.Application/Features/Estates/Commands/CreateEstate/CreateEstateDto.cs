namespace HomeHive.Application.Features.Estates.Commands.CreateEstate;

public record CreateEstateDto(Guid Id, Guid OwnerId, string? EstateType, string? EstateCategory, string? Name,
    string? Location);