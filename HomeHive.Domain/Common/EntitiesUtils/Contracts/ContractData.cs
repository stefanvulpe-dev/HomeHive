namespace HomeHive.Domain.Common.EntitiesUtils.Contracts;

public record ContractData(
    Guid EstateId,
    string? ContractType,
    string? Status,
    decimal? Price,
    DateTime? StartDate,
    DateTime? EndDate,
    string? Description);