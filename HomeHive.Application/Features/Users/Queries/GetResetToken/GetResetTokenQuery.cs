using HomeHive.Application.Contracts.Queries;

namespace HomeHive.Application.Features.Users.Queries.GetResetToken;

public record GetResetTokenQuery(Guid UserId, string Purpose): IQuery<GetResetTokenQueryResponse>;