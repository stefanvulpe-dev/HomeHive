using System.ComponentModel.DataAnnotations;

namespace HomeHive.Application.Models;

public class RegistrationModel
{
    [Required(ErrorMessage = "Username is required.")]
    [MinLength(5, ErrorMessage = "Username must be at least 5 characters.")]
    [MaxLength(15, ErrorMessage = "Username cannot be more than 25 characters.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [MinLength(3, ErrorMessage = "First name must be at least 3 characters.")]
    [MaxLength(25, ErrorMessage = "First name cannot be more than 25 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [MinLength(3, ErrorMessage = "Last name must be at least 3 characters.")]
    [MaxLength(25, ErrorMessage = "Last name cannot be more than 25 characters.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email is not valid.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$", ErrorMessage =
        "Password must be at least 8 characters, "
        + "contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "ConfirmPassword is required.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "PhoneNumber is required.")]
    [Phone(ErrorMessage = "Phone number is not valid.")]
    public string PhoneNumber { get; set; } = string.Empty;
}