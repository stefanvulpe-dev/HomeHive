using HomeHive.Application.Contracts.Commands;
using HomeHive.Domain.Common.EntitiesUtils.Estates;

namespace HomeHive.Application.Features.Estates.Commands.CreateEstate;

public record CreateEstateCommand(Guid OwnerId, CreateEstateFormData CreateEstateFormData) : ICommand<CreateEstateCommandResponse>;