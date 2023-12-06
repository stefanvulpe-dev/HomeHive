using HomeHive.Application.Contracts.Commands;
using HomeHive.Domain.Common.EntitiesUtils.Contracts;
using MediatR;

namespace HomeHive.Application.Features.Contracts.Commands.CreateContract;

public record CreateContractCommand(Guid UserId, ContractData Data) : ICommand<CreateContractCommandResponse>;
