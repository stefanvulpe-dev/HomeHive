using HomeHive.Application.Contracts.Commands;

namespace HomeHive.Application.Features.Estates.Commands.DeleteEstateById;

public record DeleteEstateByIdCommand(Guid EstateId) : ICommand<DeleteEstateByIdCommandResponse>;