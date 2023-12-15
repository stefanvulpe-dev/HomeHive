using HomeHive.Application.Contracts.Commands;
using HomeHive.Domain.Common.EntitiesUtils.Utilities;

namespace HomeHive.Application.Features.Utilities.Commands.UpdateUtility;

public record UpdateUtilityCommand(Guid UtilityId, UtilityData UtilityData) : ICommand<UpdateUtilityCommandResponse>;  
