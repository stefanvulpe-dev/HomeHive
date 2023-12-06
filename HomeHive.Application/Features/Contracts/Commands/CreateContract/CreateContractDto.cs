using HomeHive.Domain.Common.EntitiesUtils.Contracts;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Contracts.Commands.CreateContract;

public class CreateContractDto
{
    public Guid ContractId { get; set; }
    public Guid UserId { get; set; }
    public Estate? Estate { get; set; }
    public ContractType? ContractType { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
}