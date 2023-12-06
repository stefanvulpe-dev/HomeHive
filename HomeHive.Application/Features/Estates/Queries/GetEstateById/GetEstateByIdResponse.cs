using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Estates.Queries.GetEstateById;

public class GetEstateByIdResponse : BaseResponse
{
    public EstateDto? Estate { get; set; }
}