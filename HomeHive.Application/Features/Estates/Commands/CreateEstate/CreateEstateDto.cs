using HomeHive.Domain.Common.EntitiesUtils.Estates;

namespace HomeHive.Application.Features.Users.Commands.CreateEstate;

public record CreateEstateDto(Guid Id, Guid OwnerId, string? EstateType, string? EstateCategory, string Name, string Location);