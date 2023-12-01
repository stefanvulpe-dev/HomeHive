using HomeHive.Domain.Entities;

namespace HomeHive.Domain.Common.EntitiesUtils.Contracts;

public record ContractData(Estate? Estate, Guid UserId, string? ContractType, DateTime? StartDate, DateTime? EndDate,
    string? Description);