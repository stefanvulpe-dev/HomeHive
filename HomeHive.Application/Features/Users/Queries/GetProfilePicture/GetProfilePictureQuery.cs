using HomeHive.Application.Contracts.Queries;

namespace HomeHive.Application.Features.Users.Queries.GetProfilePicture;

public record GetProfilePictureQuery(Guid UserId) : IQuery<GetProfilePictureQueryResponse>;