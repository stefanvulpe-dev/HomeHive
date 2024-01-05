using System.ComponentModel.DataAnnotations;

namespace HomeHive.UI.ViewModels.Estates;

public class CreateEstateModel
{
    [Required(ErrorMessage = "Estate type is required")]
    public string? EstateType { get; set; }

    [Required(ErrorMessage = "Estate category is required")]
    public string? EstateCategory { get; set; }

    [Required(ErrorMessage = "Estate name is required")]
    [MaxLength(50, ErrorMessage = "Estate name must not exceed 50 characters")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Estate location is required")]
    public string? Location { get; set; }
    
    [Required(ErrorMessage = "Estate price is required")]
    [Range(0.0d, 1_000_000d, ErrorMessage = "Estate price must be greater than 0")]
    public decimal? Price { get; set; }

    [Required(ErrorMessage = "Estate total area is required")]
    [Range(0.0d, 100_000d, ErrorMessage = "Estate price must be greater than 0")]
    public string? TotalArea { get; set; }

    [Required(ErrorMessage = "Estate utilities is required")]
    public List<string>? Utilities { get; set; } 

    [Required(ErrorMessage = "Rooms are required")]
    public Dictionary<string, int>? EstateRooms { get; set; }

    [Required(ErrorMessage = "Estate description is required")]
    public string? Description { get; set; }
}