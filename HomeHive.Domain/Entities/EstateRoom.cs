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
}