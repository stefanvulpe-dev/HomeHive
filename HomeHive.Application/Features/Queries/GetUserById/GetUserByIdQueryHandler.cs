using HomeHive.Application.Persistence;
using MediatR;

namespace HomeHive.Application.Features.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _repository;
    
    public GetUserByIdQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.FindByIdAsync(request.Id);
        if (user.IsSuccess)
        {
            return new UserDto
            {
                FirstName = user.Value.FirstName,
                LastName = user.Value.LastName,
                Email = user.Value.Email,
                Password = user.Value.Password,
                PhoneNumber = user.Value.PhoneNumber,
                ProfilePicture = user.Value.ProfilePicture,
                Estates = user.Value.Estates
            };
        }
        return new UserDto();
    }
}
