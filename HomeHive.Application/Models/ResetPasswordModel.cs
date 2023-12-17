using System.ComponentModel.DataAnnotations;

namespace HomeHive.Application.Models;

public class ResetPasswordModel
{
    [Required(ErrorMessage = "Reset password token is required")]
    public string? ResetPasswordToken { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string? NewPassword { get; set; }
}