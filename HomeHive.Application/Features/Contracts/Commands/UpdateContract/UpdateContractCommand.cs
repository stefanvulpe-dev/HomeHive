using HomeHive.Application.Contracts.Commands;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;

namespace HomeHive.Application.Features.Contracts.Commands.UpdateContract;

public record UpdateContractCommand(Guid Id, ContractData Data) : ICommand<UpdateContractCommandResponse>;