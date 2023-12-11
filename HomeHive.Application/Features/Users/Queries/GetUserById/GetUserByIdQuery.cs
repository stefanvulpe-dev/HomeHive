using HomeHive.Application.Contracts.Queries;

namespace HomeHive.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IQuery<GetUserByIdResponse>;