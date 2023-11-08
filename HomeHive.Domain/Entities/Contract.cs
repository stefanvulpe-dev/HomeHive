using HomeHive.Domain.Common;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;

namespace HomeHive.Domain.Entities;

public sealed class Contract : BaseEntity
{
    private Contract()
    {
    }

    public Guid UserId { get; set; }
    public Guid EstateId { get; set; }
    public Estate? Estate { get; set; }
    public User? User { get; set; }
    public ContractType ContractType { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }

    public Result<Contract> Create(ContractData contractData)
    {
        var (estate, user, contractType, startDate, endDate, description) = contractData;
        if (user == null)
        {
            return Result<Contract>.Failure("User is required.");
        }

        if (estate == null)
        {
            return Result<Contract>.Failure("Estate is required.");
        }

        if (string.IsNullOrWhiteSpace(contractType) || !Enum.TryParse(contractType, out ContractType typeEnum))
        {
            return Result<Contract>.Failure("Type is not valid.");
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
            User = user,
            Estate = estate,
            UserId = user.Id,
            EstateId = estate.Id,
            ContractType = Enum.Parse<ContractType>(contractType),
            StartDate = startDate,
            EndDate = endDate,
            Description = description
        });
    }
}