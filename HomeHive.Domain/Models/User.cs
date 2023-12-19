using HomeHive.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HomeHive.Domain.Models;

public sealed class User : IdentityUser<Guid>
{
    private readonly List<Estate>? _estates = null;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePicture { get; set; }
    public string? ResetPasswordToken { get; set; }
    public DateTime? ResetPasswordTokenExpires { get; set; }
    public string? ResetEmailToken { get; set; }
    public DateTime? ResetEmailTokenExpires { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
    public IReadOnlyList<Estate>? Estates => _estates;
}