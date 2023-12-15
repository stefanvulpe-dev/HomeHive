using HomeHive.Application.Responses;

namespace HomeHive.Application.Features.Utilities.Queries.GetAllUtilities;

public class GetAllUtilitiesResponse : BaseResponse
{
    public IReadOnlyList<string>? UtilitiesNames { get; set; }
}