using HomeHive.Application.Contracts.Queries;

namespace HomeHive.Application.Features.Users.Queries.GetUserGeneralInfo;

public record GetUserGeneralInfoQuery(Guid UserId): 
    IQuery<GetUserGeneralInfoQueryResponse>;