using HomeHive.Application.Contracts.Commands;
using HomeHive.Domain.Common.EntitiesUtils.Utilities;

namespace HomeHive.Application.Features.Utilities.Commands.CreateUtility;

public record CreateUtilityCommand(UtilityData UtilityData) : ICommand<CreateUtilityCommandResponse>;