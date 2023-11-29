using HomeHive.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace HomeHive.Domain.Entities;

public sealed record UserData(string? UserName, string? FirstName, string? LastName, string? Email,
    string? Password, string? PhoneNumber, string? ProfilePicture);

public sealed class User : IdentityUser<Guid>
{
    private static readonly PasswordHasher<User> PasswordHasher = new();
    private readonly List<Estate>? _estates = null;

    private User()
    {
    }

    private User(string? userName, string? firstName, string? lastName, string? email,
        string? phoneNumber, string? profilePicture)
    {
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        ProfilePicture = profilePicture;
    }

    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? ProfilePicture { get; private set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
    public IReadOnlyList<Estate>? Estates => _estates;

    public static Result<User> Create(UserData userData)
    {
        var (userName, firstName, lastName, email,
            password, phoneNumber, profilePicture) = userData;

        if (string.IsNullOrWhiteSpace(firstName)) return Result<User>.Failure("First Name is required.");

        if (string.IsNullOrWhiteSpace(lastName)) return Result<User>.Failure("Last Name is required.");

        if (string.IsNullOrWhiteSpace(email)) return Result<User>.Failure("Email is required.");

        if (string.IsNullOrWhiteSpace(password)) return Result<User>.Failure("Password is required.");

        if (string.IsNullOrWhiteSpace(phoneNumber)) return Result<User>.Failure("Phone Number is required.");

        if (string.IsNullOrWhiteSpace(profilePicture)) return Result<User>.Failure("Profile Picture is required.");

        User user = new(userName, firstName, lastName, email, phoneNumber, profilePicture);
        var passwordHash = PasswordHasher.HashPassword(user, password);
        user.PasswordHash = passwordHash;
        return Result<User>.Success(user);
    }

    public void UpdateUser(UserData userData)
    {
        if (userData.UserName != null) UserName = userData.UserName;

        if (userData.FirstName != null) FirstName = userData.FirstName;

        if (userData.LastName != null) LastName = userData.LastName;

        if (userData.Email != null) Email = userData.Email;

        if (userData.PhoneNumber != null) PhoneNumber = userData.PhoneNumber;

        if (userData.ProfilePicture != null) ProfilePicture = userData.ProfilePicture;

        if (userData.Password != null)
        {
            var hashedPassword = PasswordHasher.HashPassword(this, userData.Password);
            PasswordHash = hashedPassword;
        }
    }
}