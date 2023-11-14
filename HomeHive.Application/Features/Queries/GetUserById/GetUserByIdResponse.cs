using MediatR;

namespace HomeHive.Application.Features.Queries.GetUserById;

public class GetUserByIdResponse : IRequest<GetUserByIdResponse>
{
    public UserDto? User { get; set; }
}