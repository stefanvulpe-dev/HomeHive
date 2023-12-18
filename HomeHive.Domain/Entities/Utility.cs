using HomeHive.Domain.Common;

namespace HomeHive.Domain.Entities;

public class Utility : BaseEntity
{
    private Utility()
    {
    }

    public string? UtilityName { get; set; }
    public List<Estate>? Estates { get; set; }

    public static Result<Utility> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return Result<Utility>.Failure("Name is required.");

        return Result<Utility>.Success(new Utility
        {
            UtilityName = name
        });
    }

    public void Update(string name)
    {
        UtilityName = name;
    }
}