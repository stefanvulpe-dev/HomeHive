using HomeHive.Application.Contracts.Commands;
using Microsoft.AspNetCore.Http;

namespace HomeHive.Application.Features.Estates.Commands.UpdateEstateAvatar;

public record UpdateEstateAvatarCommand(Guid EstateId, IFormFile EstateAvatar) : ICommand<UpdateEstateAvatarCommandResponse>;
