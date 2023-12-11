using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Features.Users.Commands.CreateEstate;
using HomeHive.Domain.Common.EntitiesUtils.Estates;

namespace HomeHive.Application.Features.Estates.Commands.CreateEstate;

public record CreateEstateCommand(Guid OwnerId, EstateData EstateData) : ICommand<CreateEstateCommandResponse>;