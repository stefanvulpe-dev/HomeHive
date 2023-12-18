using System.ComponentModel.DataAnnotations;

namespace HomeHive.UI.ViewModels;

public class ContractViewModel
{
    [Required(ErrorMessage = "Estate Id is required.")]
    public Guid EstateId { get; set; }

    [Required(ErrorMessage = "Contract Type is required.")]
    public string? ContractType { get; set; }

    [Required(ErrorMessage = "Start Date is required.")]
    public DateTime? StartDate { get; set; }

    [Required(ErrorMessage = "End Date is required.")]
    public DateTime? EndDate { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public string? Description { get; set; }
}