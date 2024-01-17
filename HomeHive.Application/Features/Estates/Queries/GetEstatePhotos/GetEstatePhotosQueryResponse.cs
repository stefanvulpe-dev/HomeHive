using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Estates.Queries.GetEstatePhotos;

public class GetEstatePhotosQueryResponse: BaseResponse
{
    public IReadOnlyList<byte[]>? EstatePhotos { get; set; }
}