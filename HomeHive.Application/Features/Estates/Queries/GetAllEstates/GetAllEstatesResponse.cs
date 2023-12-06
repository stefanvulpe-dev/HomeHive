using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Estates.Queries.GetAllEstates;

public class GetAllEstatesResponse : BaseResponse
{
    public IReadOnlyList<EstateDto>? Estates { get; set; }
}