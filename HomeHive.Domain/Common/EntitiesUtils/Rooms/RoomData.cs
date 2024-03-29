﻿using System.ComponentModel.DataAnnotations;
using HomeHive.Domain.Entities;

namespace HomeHive.Domain.Common.EntitiesUtils.Rooms;

public record RoomData
{
    public Guid EstateId { get; set; }
    
    public string? RoomType { get; set; }

    public int Quantity { get; set; }
}