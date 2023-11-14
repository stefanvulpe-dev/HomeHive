using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Queries;

public class UserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfilePicture { get; set; }
    public List<Estate>? Estates { get; set; }
}