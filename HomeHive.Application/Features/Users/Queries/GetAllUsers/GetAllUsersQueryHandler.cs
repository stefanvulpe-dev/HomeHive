using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetAllUsersQuery, GetAllUsersQueryResponse>
{
    public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync();

        if (!users.IsSuccess)
            return new GetAllUsersQueryResponse
            {
                IsSuccess = false,
                Message = "Users not found."
            };

        IReadOnlyList<UserDto>? mappedUsers = users.Value.Select(user =>
            new UserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            }).ToList();

        return new GetAllUsersQueryResponse { Users = mappedUsers };
    }
}