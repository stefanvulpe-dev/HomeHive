using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace HomeHive.UI.ViewModels.Estates;

public class EditEstateAvatarModel
{
    [Required(ErrorMessage = "EstateId is required.")]
    public Guid EstateId { get; set; }
    
    [Required(ErrorMessage = "Please select an image to upload.")]
    public IBrowserFile? EstateAvatar { get; set; }
}