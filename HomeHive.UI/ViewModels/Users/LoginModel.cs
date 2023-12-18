using System.ComponentModel.DataAnnotations;

namespace HomeHive.UI.ViewModels.Users;

public class LoginModel
{
    [Required(ErrorMessage = "Username is required.")]
    [MinLength(5, ErrorMessage = "Username must be at least 5 characters.")]
    [MaxLength(15, ErrorMessage = "Username cannot be more than 25 characters.")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$", ErrorMessage =
        "Password must be at least 8 characters, "
        + "contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")]
    public string? Password { get; set; }
}