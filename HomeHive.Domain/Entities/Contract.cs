using HomeHive.Domain.Common;

namespace HomeHive.Domain.Entities;

public sealed class Contract: BaseEntity
{
    private Contract()
    {
    }
    
    public Guid UserId { get; set; }
    public Guid EstateId { get; set; }
    public ContractType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Description { get; set; }

    public Result<Contract> Create(Guid userId, Guid estateId, ContractType type, DateTime startDate, DateTime endDate,
        string description)
    {
        if (userId == default)
        {
            return Result<Contract>.Failure("User id should not be default!");
        }

        if (estateId == default)
        {
            return Result<Contract>.Failure("Estate id should not be default!");
        }

        if (startDate == default)
        {
            return Result<Contract>.Failure("Start date should not be default!");
        }
        
        if (endDate == default)
        {
            return Result<Contract>.Failure("End date should not be default!");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            return Result<Contract>.Failure("Description is not valid!");
        }

        return Result<Contract>.Success(new Contract
        {
            UserId = userId,
            EstateId = estateId,
            Type = type,
            StartDate = startDate,
            EndDate = endDate,
            Description = description
        });
    }
}

public enum ContractType
{
    Rent,
    Sale
}