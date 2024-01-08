using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;

namespace HomeHive.Application.Features.Estates.Commands.UpdateEstatePhotos;

public class UpdateEstatePhotosCommandHandler(IEstateRepository estateRepository, IEstatePhotosRepository estatePhotosRepository, IBlobStorageService blobStorageService): ICommandHandler<UpdateEstatePhotosCommand, UpdateEstatePhotosCommandResponse>
{
    public async Task<UpdateEstatePhotosCommandResponse> Handle(UpdateEstatePhotosCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateEstatePhotosCommandValidator(estateRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors
                .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());

            return new UpdateEstatePhotosCommandResponse
            {
                IsSuccess = false,
                Message = "Failed to update estate photos",
                ValidationsErrors = validationErrors
            };
        }
        
        var existingEstate = validator.Estate!;
        
        var estatePhotosResult = await estatePhotosRepository.FindAllByEstateIdAsync(request.EstateId);
        
        var existingEstatePhotos = estatePhotosResult.Value;
        
        if (existingEstatePhotos.Count > 0)
        {
            var deleteResult = await estatePhotosRepository.DeleteAllByEstateIdAsync(request.EstateId);
            if (!deleteResult.IsSuccess)
                return new UpdateEstatePhotosCommandResponse
                {
                    IsSuccess = false,
                    Message = $"Failed to delete existing estate photos for estate {request.EstateId}"
                };

            foreach (var estatePhoto in existingEstatePhotos)
            {
                await blobStorageService.DeleteBlobAsync(estatePhoto.ObjectName!, cancellationToken);
            }
        }
        
        foreach (var photo in request.EstatePhotos)
        {
            var objectName = Guid.NewGuid().ToString();
            
            var blobResult =
                await blobStorageService.UploadBlobAsync(objectName, photo.OpenReadStream(),
                    cancellationToken);
            if (!blobResult)
                return new UpdateEstatePhotosCommandResponse
                {
                    IsSuccess = false,
                    Message = $"Failed to upload estate photo {photo.FileName}"
                };
            
            var estatePhotoResult = EstatePhoto.Create(objectName, existingEstate);
            if (!estatePhotoResult.IsSuccess)
                return new UpdateEstatePhotosCommandResponse
                {
                    IsSuccess = false,
                    Message = $"Failed to create estate photo {photo.FileName}",
                    ValidationsErrors = new Dictionary<string, List<string>> { { "EstatePhoto",  estatePhotoResult.ValidationErrors!.Select(er => er.Value).ToList() } }
                };
            
            var result = await estatePhotosRepository.AddAsync(estatePhotoResult.Value);
            if (!result.IsSuccess)
                return new UpdateEstatePhotosCommandResponse
                {
                    IsSuccess = false,
                    Message = $"Failed to add estate photo {photo.FileName}"
                };
        }
        
        return new UpdateEstatePhotosCommandResponse
        {
            IsSuccess = true,
            Message = "Estate photos updated successfully",
        };
    }
}