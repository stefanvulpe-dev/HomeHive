using System.ComponentModel.DataAnnotations;

namespace HomeHive.Application.Models;

public class UpdateEmailModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string? NewEmail { get; set; }

    [Required(ErrorMessage = "Email reset token is required")]
    public string? EmailResetToken { get; set; }
}