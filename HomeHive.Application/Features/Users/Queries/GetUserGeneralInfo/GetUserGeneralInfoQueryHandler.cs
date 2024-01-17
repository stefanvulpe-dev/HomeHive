using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Users.Queries.GetUserGeneralInfo;

public class GetUserGeneralInfoQueryHandler(IUserRepository userRepository): 
    IQueryHandler<GetUserGeneralInfoQuery, GetUserGeneralInfoQueryResponse>
{
    public async Task<GetUserGeneralInfoQueryResponse> Handle(GetUserGeneralInfoQuery request,
        CancellationToken cancellationToken)
    {
        var userResult = await userRepository.FindByIdAsync(request.UserId);

        if (!userResult.IsSuccess)
            return new GetUserGeneralInfoQueryResponse
            {
                IsSuccess = false,
                Message = "User not found."
            };

        return new GetUserGeneralInfoQueryResponse
        {
            IsSuccess = true,
            User = new UserDto
            {
                UserName = userResult.Value.UserName,
                Email = userResult.Value.Email,
                FirstName = userResult.Value.FirstName,
                LastName = userResult.Value.LastName,
            }
        };
    }
}