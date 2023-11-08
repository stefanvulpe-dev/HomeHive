using HomeHive.Domain.Entities;

namespace HomeHive.Domain.Common.EntitiesUtils.Contracts;

public record ContractData(Estate? Estate, User? User, string? ContractType, DateTime? StartDate, DateTime? EndDate,
    string? Description);