using MediatR;

namespace HomeHive.Application.Features.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto>, IRequest<GetUserByIdResponse>; 