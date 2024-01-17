using System.Text.Json.Serialization;

namespace HomeHive.UI.ViewModels.Contracts;

public class ContractViewModel
{
    public Guid? Id { get; set; }
    
    public Guid? EstateId { get; set; }
    
    public Guid? OwnerId { get; set; }
    
    public string? ContractType { get; set; }
    
    public string? Status { get; init; }
    
    public decimal? Price { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public string? Description { get; set; }
    
    [JsonIgnore]
    public string? OwnerName { get; set; }
    
    [JsonIgnore]
    public string? EstateName { get; set; }
    
    [JsonIgnore]
    public string? ClientName { get; set; }
}