namespace HomeHive.Application.Features.Estates.Commands.CreateEstate;

public record CreateEstateDto
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }
    public string? EstateType { get; init; }
    public string? EstateCategory { get; init; }
    public string? Name { get; init; }
    public string? Location { get; init; }
}