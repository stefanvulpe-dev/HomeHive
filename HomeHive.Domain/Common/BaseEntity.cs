namespace HomeHive.Domain.Common;

public class BaseEntity
{
    public Guid Id { get; } = Guid.NewGuid();
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string? LastModifiedBy { get; set; }
    public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
}