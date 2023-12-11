using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserRepository repository) : IQueryHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userResult = await repository.FindByIdAsync(request.Id);

        if (!userResult.IsSuccess)
            return new GetUserByIdResponse
            {
                Success = false,
                Message = "User not found."
            };

        return new GetUserByIdResponse
        {
            Success = true,
            User = new UserDto(userResult.Value.UserName, userResult.Value.FirstName, userResult.Value.LastName,
                userResult.Value.Email, userResult.Value.PhoneNumber)
        };
    }
}