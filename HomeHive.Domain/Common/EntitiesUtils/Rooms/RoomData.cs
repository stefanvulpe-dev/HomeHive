using HomeHive.Domain.Entities;

namespace HomeHive.Domain.Common.EntitiesUtils.Rooms;

public record RoomData(string? RoomType, int Capacity, int Size);