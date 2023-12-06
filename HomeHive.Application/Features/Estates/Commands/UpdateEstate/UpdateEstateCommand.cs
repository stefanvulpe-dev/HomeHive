using HomeHive.Application.Contracts.Commands;
using HomeHive.Domain.Common.EntitiesUtils.Estates;

namespace HomeHive.Application.Features.Estates.Commands.UpdateEstate;

public record UpdateEstateCommand(Guid EstateId, EstateData EstateData) : ICommand<UpdateEstateCommandResponse>;