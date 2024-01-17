namespace HomeHive.Domain.Common.EntitiesUtils.Contracts;

public record ContractData(
    Guid EstateId,
    Guid OwnerId,
    string? ContractType,
    string? Status,
    decimal? Price,
    DateTime? StartDate,
    DateTime? EndDate,
    string? Description);