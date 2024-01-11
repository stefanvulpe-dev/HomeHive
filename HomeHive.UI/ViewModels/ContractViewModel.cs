using System.ComponentModel.DataAnnotations;

namespace HomeHive.UI.ViewModels;

public class ContractViewModel
{
    [Required(ErrorMessage = "Estate id is required")]
    public Guid? EstateId { get; set; }
    
    [Required(ErrorMessage = "Contract type is required")]
    public string? ContractType { get; set; }
    
    public string? Status { get; init; } = "Pending";
    
    [Required(ErrorMessage = "Price is required")]
    [Range(0, 1000000000, ErrorMessage = "Price must be between 0 and 1,000,000,000")]
    public decimal? Price { get; set; }
    
    [Required(ErrorMessage = "Start date is required")]
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    [Required(ErrorMessage = "Please enter something here")]
    [MaxLength(250, ErrorMessage = "Your description is too long (250 characters max).")]
    public string? Description { get; set; }

}