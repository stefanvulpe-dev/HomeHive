using System.ComponentModel.DataAnnotations;

namespace HomeHive.UI.ViewModels.Estates;

public class EditEstateModel
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
    
    public string PriceString { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Estate price is required")]
    [Range(0.0d, 1_000_000d, ErrorMessage = "Estate price must be greater than 0")]
    public decimal? Price => decimal.TryParse(PriceString, out var price) ? price : null;

    [Required(ErrorMessage = "Estate total area is required")]
    [Range(0.0d, 100_000d, ErrorMessage = "Estate price must be greater than 0")]
    public string? TotalArea { get; set; }

    public string[] EstateUtilities { get; set; } = Array.Empty<string>();
    
    [Required(ErrorMessage = "Estate utilities are required")]
    [MinLength(1, ErrorMessage = "Estate utilities must be at least 1")]
    public List<string> Utilities => EstateUtilities.ToList();
    
    [MinLength(1, ErrorMessage = "Estate rooms must be at least 1")]
    public string[] EstateRoomsKeys { get; set; } = Array.Empty<string>();
    
    [Required(ErrorMessage = "Rooms are required")]
    [MinLength(1, ErrorMessage = "Rooms must be at least 1")]
    public Dictionary<string, int>? Rooms { get; set; }

    [Required(ErrorMessage = "Estate description is required")]
    public string? Description { get; set; }
}