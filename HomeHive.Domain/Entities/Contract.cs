using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;

namespace HomeHive.Domain.Entities;

public sealed class Contract : BaseEntity
{
    private Contract()
    {
    }

    public Guid UserId { get; private set; }
    public Guid EstateId { get; private set; }
    public Estate? Estate { get; private set; }
    public ContractType? ContractType { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? Description { get; private set; }
    public ContractStatus? Status { get; private set; }
    public decimal? Price { get; private set; }

    public static Result<Contract> Create(Guid userId, ContractData contractData)
    {
        var (estateId, contractType, contractStatus, price, startDate, endDate, description) = contractData;

        if (estateId == Guid.Empty) return Result<Contract>.Failure("EstateId is required.");

        if (userId == Guid.Empty) return Result<Contract>.Failure("UserId is required.");

        if (string.IsNullOrWhiteSpace(contractType) || !Enum.TryParse(contractType, out ContractType typeEnum))
            return Result<Contract>.Failure("Type is not valid.");

        if (string.IsNullOrWhiteSpace(contractStatus) ||
            !Enum.TryParse(contractStatus, out ContractStatus statusEnum))
            return Result<Contract>.Failure("Status is not valid.");
        
        if (startDate == default) return Result<Contract>.Failure("Start date should not be default!");
        
        if (string.IsNullOrWhiteSpace(description)) return Result<Contract>.Failure("Description is not valid!");
        
        if (price == default) return Result<Contract>.Failure("Price should not be default!");
        
        return Result<Contract>.Success(new Contract
        {
            Estate = null,
            UserId = userId,
            EstateId = estateId,
            ContractType = Enum.Parse<ContractType>(contractType),
            Status = Enum.Parse<ContractStatus>(contractStatus),
            Price = price,
            StartDate = startDate,
            EndDate = endDate,
            Description = description
        });
    }

    public void Update(ContractData data)
    {
        if (data.EstateId != Guid.Empty) EstateId = data.EstateId;

        if (data.Description != null) Description = data.Description;

        if (data.StartDate != null) StartDate = data.StartDate;

        if (data.EndDate != null) EndDate = data.EndDate;

        if (data.Price != null) Price = data.Price;
        
        if (data.ContractType != null) ContractType = Enum.Parse<ContractType>(data.ContractType);
        
        if (data.Status != null) Status = Enum.Parse<ContractStatus>(data.Status);
    }
}