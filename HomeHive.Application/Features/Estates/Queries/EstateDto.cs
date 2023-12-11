namespace HomeHive.Application.Features.Estates.Queries;

public record EstateDto
{
    public Guid OwnerId { get; init; }
    public string? EstateType { get; init; }
    public string? EstateCategory { get; init; }
    public string? Name { get; init; }
    public string? Location { get; init; }
    public decimal? Price { get; init; }
    public string? TotalArea { get; init; }
    public string? Utilities { get; init; }
    public string? Description { get; init; }
    public string? Image { get; init; }
}