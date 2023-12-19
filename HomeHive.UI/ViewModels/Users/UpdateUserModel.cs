using System.ComponentModel.DataAnnotations;

namespace HomeHive.UI.ViewModels.Users;

public class UpdateUserModel
{
    [Required(ErrorMessage = "First name is required.")]
    [MinLength(3, ErrorMessage = "First name must be at least 3 characters.")]
    [MaxLength(30, ErrorMessage = "First name cannot be more than 30 characters.")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [MinLength(3, ErrorMessage = "Last name must be at least 3 characters.")]
    [MaxLength(30, ErrorMessage = "Last name cannot be more than 30 characters.")]
    public string? LastName { get; set; }
}