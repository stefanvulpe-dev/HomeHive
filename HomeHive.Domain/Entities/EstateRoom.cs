using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HomeHive.Domain.Common;

namespace HomeHive.Domain.Entities;

public class EstateRoom: BaseEntity
{
    private EstateRoom()
    {
        
    }
    
    [ForeignKey("Estate")]
    public Guid EstateId { get; private set; }
    
    [ForeignKey("Room")]
    public Guid RoomId { get; private set; }

    public int Quantity { get; private set; } = 1;
    
    public Estate? Estate { get; private set; }
    public Room? Room { get; private set; }
    
    public static Result<EstateRoom> Create(Guid estateId, Guid roomId, int quantity)
    {
        if (estateId == Guid.Empty) return Result<EstateRoom>.Failure("Estate is required.");
        if (roomId == Guid.Empty) return Result<EstateRoom>.Failure("Room is required.");
        if (quantity <= 0) return Result<EstateRoom>.Failure("Quantity must be greater than 0.");
        
        return Result<EstateRoom>.Success(new EstateRoom { EstateId = estateId, RoomId = roomId, Quantity = quantity });
    }
    
    public void Update(Guid estateId, Guid roomId, int quantity)
    {
        if (estateId != Guid.Empty) 
            EstateId = estateId;
        if (roomId != Guid.Empty)
            RoomId = roomId;
        if (quantity > 0)
            Quantity = quantity;
    }
}