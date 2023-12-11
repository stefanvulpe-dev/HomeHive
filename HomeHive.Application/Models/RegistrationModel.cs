using System.ComponentModel.DataAnnotations;

namespace HomeHive.Application.Models;

public class RegistrationModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    public string? LastName { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Phone Number is required")]
    public string? PhoneNumber { get; set; }
}