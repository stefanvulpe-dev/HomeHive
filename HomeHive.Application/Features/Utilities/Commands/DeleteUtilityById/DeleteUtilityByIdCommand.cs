using HomeHive.Application.Contracts.Commands;

namespace HomeHive.Application.Features.Utilities.Commands.DeleteUtilityById;

public record DeleteUtilityByIdCommand(Guid UtilityId) : ICommand<DeleteUtilityByIdCommandResponse>;