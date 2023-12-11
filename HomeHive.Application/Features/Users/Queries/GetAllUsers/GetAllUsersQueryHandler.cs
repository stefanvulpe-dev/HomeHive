using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler
    (IUserRepository userRepository) : IQueryHandler<GetAllUsersQuery, GetAllUsersQueryResponse>
{
    public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync();
        
        if (!users.IsSuccess)
            return new GetAllUsersQueryResponse
            {
                Success = false,
                Message = "Users not found."
            };
        
        IReadOnlyList<UserDto>? mappedUsers = users.Value.Select(user =>
            new UserDto(user.UserName, user.FirstName, user.LastName, user.Email, user.PhoneNumber)).ToList();
        
        return new GetAllUsersQueryResponse { Users = mappedUsers };
    }
}