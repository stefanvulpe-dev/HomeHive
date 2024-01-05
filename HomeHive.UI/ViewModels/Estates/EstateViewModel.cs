﻿namespace HomeHive.UI.ViewModels.Estates;

public class EstateViewModel
{
    public string? Id { get; set; }
    
    public string? OwnerId { get; set; }
    
    public string? EstateType { get; set; }

    public string? EstateCategory { get; set; }

    public string? Name { get; set; }

    public string? Location { get; set; }

    public decimal? Price { get; set; }

    public string? TotalArea { get; set; }

    public List<string?> Utilities { get; set; }

    public Dictionary<string, int>? Rooms { get; set; }
    
    public string? Description { get; set; }

    public string? Image { get; set; }
}