namespace HomeHive.Application.Features.Commands.CreateUser;

public record CreateUserDto(Guid? Id, string? FirstName, string? LastName, string? Email);