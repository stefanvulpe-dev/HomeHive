namespace HomeHive.Application.Features.Utilities.Commands;

public record CreateUtilityDto
{
    public Guid Id { get; init; }
    public string? UtilityName { get; init; }
}