using HomeHive.Application.Contracts.Commands;
using HomeHive.Domain.Common.EntitiesUtils.Estates;
using MediatR;

namespace HomeHive.Application.Features.Users.Commands.CreateEstate;

public record CreateEstateCommand(EstateData EstateData) : ICommand<CreateEstateCommandResponse>;