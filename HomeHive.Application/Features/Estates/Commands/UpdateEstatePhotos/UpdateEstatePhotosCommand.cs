using HomeHive.Application.Contracts.Commands;
using Microsoft.AspNetCore.Http;

namespace HomeHive.Application.Features.Estates.Commands.UpdateEstatePhotos;

public record UpdateEstatePhotosCommand(Guid EstateId, List<IFormFile> EstatePhotos) : ICommand<UpdateEstatePhotosCommandResponse>;
