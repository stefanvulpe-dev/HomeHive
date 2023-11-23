using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Estates;

namespace HomeHive.Domain.Entities;

public sealed record UserData(string? FirstName, string? LastName, string? Email, 
                        string? Password, string? PhoneNumber, string? ProfilePicture);

public sealed class User : BaseEntity
{
    private readonly List<Estate>? _estates = null;
    
    private User()
    {
    }

    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Email { get; private set; }
    public string? Password { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? ProfilePicture { get; private set; }
    public IReadOnlyList<Estate>? Estates => _estates;
    
    public static Result<User> Create(UserData userData)
    {
        var (firstName, lastName, email, 
            password, phoneNumber, profilePicture) = userData;
        
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
        });
    }
    
    public void UpdateUser(UserData userData)
    {
        if (userData.FirstName != null)
        {
            FirstName = userData.FirstName;
        }

        if (userData.LastName != null)
        {
            LastName = userData.LastName;
        }

        if (userData.Email != null)
        {
            Email = userData.Email;
        }

        if (userData.Password != null)
        {
            Password = userData.Password;
        }

        if (userData.PhoneNumber != null)
        {
            PhoneNumber = userData.PhoneNumber;
        }

        if (userData.ProfilePicture != null)
        {
            ProfilePicture = userData.ProfilePicture;
        }
    }

}