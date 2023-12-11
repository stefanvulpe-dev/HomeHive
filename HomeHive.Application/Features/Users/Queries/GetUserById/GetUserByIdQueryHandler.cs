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
                IsSuccess = false,
                Message = "User not found."
            };

        return new GetUserByIdResponse
        {
            IsSuccess = true,
            User = new UserDto
            {
                UserName = userResult.Value.UserName,
                Email = userResult.Value.Email,
                FirstName = userResult.Value.FirstName,
                LastName = userResult.Value.LastName,
                PhoneNumber = userResult.Value.PhoneNumber
            }
        };
    }
}