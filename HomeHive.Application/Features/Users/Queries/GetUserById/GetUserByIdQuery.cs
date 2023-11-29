using HomeHive.Application.Abstractions;

namespace HomeHive.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IQuery<GetUserByIdResponse>;