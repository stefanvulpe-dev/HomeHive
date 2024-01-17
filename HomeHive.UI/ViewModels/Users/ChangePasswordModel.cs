using System.ComponentModel.DataAnnotations;

namespace HomeHive.UI.ViewModels.Users;

public class ChangePasswordModel
{
    [Required(ErrorMessage = "Token is required.")]
    public string? ResetPasswordToken { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$", ErrorMessage =
        "Password must be at least 8 characters, "
        + "contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")]
    public string? NewPassword { get; set; }

    [Required(ErrorMessage = "ConfirmPassword is required.")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    public string? ConfirmPassword { get; set; }
}