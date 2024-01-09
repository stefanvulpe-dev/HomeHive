namespace HomeHive.UI.ViewModels.Estates;

public class EstateViewModel
{
    public Guid? Id { get; set; }
    
    public Guid? OwnerId { get; set; }
    
    public string? EstateType { get; set; }

    public string? EstateCategory { get; set; }

    public string? Name { get; set; }

    public string? Location { get; set; }

    public decimal? Price { get; set; }

    public string? TotalArea { get; set; }

    public List<string?>? Utilities { get; set; }

    public Dictionary<string, int>? Rooms { get; set; }
    
    public string? Description { get; set; }

    public string? EstateAvatar { get; set; }
    
    public List<string>? EstatePhotos { get; set; }
}