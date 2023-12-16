namespace HomeHive.Application.Features.Utilities.Commands.CreateUtility;

public record CreateUtilityDto
{
    public Guid Id { get; init; }
    public string? UtilityName { get; init; }
}