using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Features.Users.Commands.DeleteUserById;

namespace HomeHive.Application.Features.Estates.Commands.DeleteEstateById;

public record DeleteEstateByIdCommand(Guid EstateId) : ICommand<DeleteEstateByIdCommandResponse>;
