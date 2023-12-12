using System.ComponentModel.DataAnnotations;

namespace HomeHive.UI.ViewModels;

public class EstateViewModel
{
    [Required(ErrorMessage = "Estate Type is required.")]
    public string? EstateType { get; set; }

    [Required(ErrorMessage = "Estate Category is required.")]
    public string? EstateCategory { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Location is required.")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    public decimal? Price { get; set; }

    [Required(ErrorMessage = "Total Area is required.")]
    public string? TotalArea { get; set; }

    [Required(ErrorMessage = "Utilities is required.")]
    public string? Utilities { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Image is required.")]
    public string? Image { get; set; }
}