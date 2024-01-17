using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace HomeHive.UI.ViewModels.Estates;

public class AddEstateImagesModel
{
    [Required(ErrorMessage = "Estate photos are required.")]
    [MinLength(5, ErrorMessage = "Please select at least 5 photos.")]
    [MaxLength(10, ErrorMessage = "Please select no more than 10 photos.")]
    public List<IBrowserFile>? EstatePhotos { get; init; }
}