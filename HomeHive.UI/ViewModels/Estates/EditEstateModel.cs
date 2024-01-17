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

    public string? Country { get; set; }
    
    public string? City { get; set; }
    
    [Required(ErrorMessage = "Estate location is required")]
    public string? Location => !string.IsNullOrWhiteSpace(Country) && !string.IsNullOrWhiteSpace(City) ? $"{Country}, {City}" : null;
    
    [Required(ErrorMessage = "Estate price is required")]
    [Range(0.0d, 1_000_000d, ErrorMessage = "Estate price must be greater than 0 and less than 1_000_000")]
    public decimal? Price { get; set; } 
    
    public int? TotalAreaInt { get; set; }
    
    [Required(ErrorMessage = "Estate total area is required")]
    public string? TotalArea => TotalAreaInt == null ? null : $"{TotalAreaInt}";

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