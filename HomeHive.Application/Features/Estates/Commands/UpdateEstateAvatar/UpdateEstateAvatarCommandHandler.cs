using HomeHive.Application.Contracts.Commands;
using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Estates.Commands.UpdateEstateAvatar;

public class UpdateEstateAvatarCommandHandler(
    IEstateRepository estateRepository,
    IBlobStorageService blobStorageService)
    : ICommandHandler<UpdateEstateAvatarCommand, UpdateEstateAvatarCommandResponse>
{
    public async Task<UpdateEstateAvatarCommandResponse> Handle(UpdateEstateAvatarCommand request,
        CancellationToken cancellationToken)
    {
        var validator = new UpdateEstateAvatarCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors
                .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToList());

            return new UpdateEstateAvatarCommandResponse
            {
                IsSuccess = false,
                Message = "Failed to update estate avatar.",
                ValidationsErrors = validationErrors
            };
        }

        var estateResult = await estateRepository.FindByIdAsync(request.EstateId);

        if (!estateResult.IsSuccess)
            return new UpdateEstateAvatarCommandResponse
            {
                IsSuccess = false,
                Message = $"Estate {request.EstateId} not found"
            };

        var existingEstate = estateResult.Value;

        await blobStorageService.DeleteBlobAsync(existingEstate.EstateAvatar!, cancellationToken);
        
        var newBlobName = Guid.NewGuid().ToString();

        var blobResult =
            await blobStorageService.UploadBlobAsync(newBlobName, request.EstateAvatar.OpenReadStream(),
                cancellationToken);

        if (!blobResult)
            return new UpdateEstateAvatarCommandResponse
            {
                IsSuccess = false,
                Message = $"Failed to upload estate avatar {request.EstateAvatar.FileName}"
            };
        
        existingEstate.UpdateEstateAvatar(newBlobName);
        
        var updateResult = await estateRepository.UpdateAsync(existingEstate);
        
        if (!updateResult.IsSuccess)
            return new UpdateEstateAvatarCommandResponse
            {
                IsSuccess = false,
                Message = $"Failed to update estate avatar {request.EstateAvatar.FileName}"
            };
        
        using var memoryStream = new MemoryStream();
        
        await request.EstateAvatar.CopyToAsync(memoryStream, cancellationToken);
        
        var bytes = memoryStream.ToArray();
        
        var base64 = Convert.ToBase64String(bytes);
        
        var imgSrc = $"data:image/png;base64,{base64}";
        
        return new UpdateEstateAvatarCommandResponse
        {
            IsSuccess = true,
            Message = $"Estate avatar {request.EstateAvatar.FileName} updated successfully."
        };
    }
}