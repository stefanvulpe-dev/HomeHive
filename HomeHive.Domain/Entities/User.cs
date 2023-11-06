using HomeHive.Domain.Common;

namespace HomeHive.Domain.Entities;

public sealed class User : BaseEntity
{
    private User()
    {
    }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfilePicture { get; set; }
    public ICollection<Estate>? Estates { get; set; }
    
    public static Result<User> Create(string firstName, string lastName, string email, string password,
        string phoneNumber, string profilePicture)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result<User>.Failure("First Name is required.");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result<User>.Failure("Last Name is required.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return Result<User>.Failure("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            return Result<User>.Failure("Password is required.");
        }

        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return Result<User>.Failure("Phone Number is required.");
        }

        if (string.IsNullOrWhiteSpace(profilePicture))
        {
            return Result<User>.Failure("Profile Picture is required.");
        }

        return Result<User>.Success(new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
            PhoneNumber = phoneNumber,
            ProfilePicture = profilePicture,
            Estates = new List<Estate>()
        });
    }
}