using HomeHive.Domain.Common.EntitiesUtils.Contracts;

namespace HomeHive.Application.Features.Contracts.Queries;

public record ContractDto(Guid UserId, Guid EstateId, string? ContractType, DateTime? StartDate, DateTime? EndDate, string? Description);