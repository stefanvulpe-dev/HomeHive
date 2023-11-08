using HomeHive.Domain.Entities;

namespace HomeHive.Domain.Common.EntitiesUtils.Rooms;

public record RoomData(string? Name, string? RoomType, int Capacity, int Size, Estate? Estate);
