using HomeHive.Application.Abstractions;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    private readonly IUserRepository _repository;
    
    public GetUserByIdQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userResult = await _repository.FindByIdAsync(request.Id);
        
        if (!userResult.IsSuccess)
        {
            return new GetUserByIdResponse
            {
                Success = false,
                Message = "User not found."
            };
        }

        return new GetUserByIdResponse
        {
            Success = true,
            User = new UserDto(userResult.Value.FirstName, userResult.Value.LastName, userResult.Value.Email, userResult.Value.PhoneNumber)
        };
    }
}
