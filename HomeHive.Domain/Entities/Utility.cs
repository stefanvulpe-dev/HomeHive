using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Utility;

namespace HomeHive.Domain.Entities;

public class Utility : BaseEntity
{
    private Utility()
    {
    }

    public UtilityType UtilityType { get; set; }
    public List<Estate>? Estates { get; set; }

    public static Result<Utility> Create(string utilityType)
    {
        if (string.IsNullOrWhiteSpace(utilityType) || !Enum.TryParse(utilityType, out UtilityType typeEnum))
            return Result<Utility>.Failure("UtilityType is not valid.");

        return Result<Utility>.Success(new Utility
        {
            UtilityType = Enum.Parse<UtilityType>(utilityType)
        });
    }

    public void Update(string utilityType)
    {
        if (string.IsNullOrWhiteSpace(utilityType) || !Enum.TryParse(utilityType, out UtilityType typeEnum))
            return;

        UtilityType = typeEnum;
    }
}