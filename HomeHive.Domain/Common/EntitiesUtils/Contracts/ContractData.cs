using HomeHive.Domain.Entities;

namespace HomeHive.Domain.Common.EntitiesUtils.Contracts;

public record ContractData(Guid EstateId, string? ContractType, DateTime? StartDate, DateTime? EndDate,
    string? Description);