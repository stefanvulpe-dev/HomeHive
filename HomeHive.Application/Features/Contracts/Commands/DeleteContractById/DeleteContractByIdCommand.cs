using HomeHive.Application.Contracts.Commands;

namespace HomeHive.Application.Features.Contracts.Commands.DeleteContractById;

public record DeleteContractByIdCommand(Guid ContractId): ICommand<DeleteContractByIdCommandResponse>;
