namespace HomeHive.Application.Features.Contracts.Queries;

public record ContractDto
{
    public Guid UserId { get; init; }
    public Guid EstateId { get; init; }
    public string? ContractType { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? Description { get; init; }
}