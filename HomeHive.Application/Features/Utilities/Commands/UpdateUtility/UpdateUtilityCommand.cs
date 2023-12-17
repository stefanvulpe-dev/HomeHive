using HomeHive.Application.Contracts.Commands;

namespace HomeHive.Application.Features.Utilities.Commands.UpdateUtility;

public record UpdateUtilityCommand(Guid UtilityId, string UtilityName) : ICommand<UpdateUtilityCommandResponse>;  
