using HomeHive.Application.Contracts.Commands;

namespace HomeHive.Application.Features.Utilities.Commands.CreateUtility;

public record CreateUtilityCommand(string UtilityName) : ICommand<CreateUtilityCommandResponse>;