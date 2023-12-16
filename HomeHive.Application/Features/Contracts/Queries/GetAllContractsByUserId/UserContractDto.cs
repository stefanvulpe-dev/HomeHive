namespace HomeHive.Application.Features.Contracts.Queries.GetAllContractsByUserId;

public class UserContractDto
{
    public Guid EstateId { get; init; }
    public string? ContractType { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? Description { get; init; }
}