namespace HomeHive.Application.Features.Users.Commands.CreateUser;

public record CreateUserDto(Guid Id, string? FirstName, string? LastName, string? Email);