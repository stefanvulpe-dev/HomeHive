using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Estates.Queries.GetEstatePhotos;

public class GetEstatePhotosQueryHandler(IEstateRepository estateRepository, IEstatePhotosRepository estatePhotosRepository, IBlobStorageService blobStorageService) : IQueryHandler<GetEstatePhotosQuery, GetEstatePhotosQueryResponse>
{
    public async Task<GetEstatePhotosQueryResponse> Handle(GetEstatePhotosQuery request, CancellationToken cancellationToken)
    {
        var estateResult = await estateRepository.FindByIdAsync(request.EstateId);
        
        if (!estateResult.IsSuccess)
            return new GetEstatePhotosQueryResponse { IsSuccess = false, Message = $"Estate with id: {request.EstateId} was not found" };
        
        var estatePhotosResult = await estatePhotosRepository.FindAllByEstateIdAsync(request.EstateId);
        
        if (!estatePhotosResult.IsSuccess)
            return new GetEstatePhotosQueryResponse { IsSuccess = false, Message = $"Estate photos for estate with id: {request.EstateId} were not found" };

        var estatePhotos = new List<byte[]>();

        foreach (var estatePhoto in estatePhotosResult.Value)
        {
            var photo = await blobStorageService.GetBlobAsync(estatePhoto.ObjectName!, cancellationToken);
            
            if (!photo.IsSuccess)
                return new GetEstatePhotosQueryResponse { IsSuccess = false, Message = $"Estate photo with name: {estatePhoto.ObjectName} was not found" };

            using var memoryStream = new MemoryStream();
            await photo.Value.CopyToAsync(memoryStream, cancellationToken);
            estatePhotos.Add(memoryStream.ToArray());
        }
        
        return new GetEstatePhotosQueryResponse { IsSuccess = true, EstatePhotos = estatePhotos };
    }
}