using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Utilities.Queries.GetAllUtilities;

public class GetAllUtilitiesQueryHandler(IUtilityRepository utilityRepository)
    : IQueryHandler<GetAllUtilitiesQuery, GetAllUtilitiesResponse>
{
    public async Task<GetAllUtilitiesResponse> Handle(GetAllUtilitiesQuery request, CancellationToken cancellationToken)
    {
        var utilities = await utilityRepository.GetAllAsync();
        
        if (!utilities.IsSuccess)
            return new GetAllUtilitiesResponse
            {
                IsSuccess = false,
                Message = "Utilities not found."
            };
        
        IReadOnlyList<string> utilitiesNames = utilities.Value.Select(u => u.UtilityName!).ToList();

        return new GetAllUtilitiesResponse
        {
            UtilitiesNames = utilitiesNames
        };
    }
}