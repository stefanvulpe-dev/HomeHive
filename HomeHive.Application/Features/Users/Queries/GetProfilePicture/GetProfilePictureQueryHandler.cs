using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;

namespace HomeHive.Application.Features.Users.Queries.GetProfilePicture;

public class GetProfilePictureQueryHandler(IUserRepository userRepository, IBlobStorageService blobStorageService)
    : IQueryHandler<GetProfilePictureQuery, GetProfilePictureQueryResponse>
{
    public async Task<GetProfilePictureQueryResponse> Handle(GetProfilePictureQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.UserId);

        if (!user.IsSuccess)
            return new GetProfilePictureQueryResponse { IsSuccess = false, Message = "User not found" };

        if (user.Value.ProfilePicture == null)
            return new GetProfilePictureQueryResponse
                { IsSuccess = false, Message = "User has no profile picture set" };

        var profilePicture = await blobStorageService.GetBlobAsync(user.Value.ProfilePicture, cancellationToken);

        return !profilePicture.IsSuccess
            ? new GetProfilePictureQueryResponse { IsSuccess = false, Message = "Profile picture not found" }
            : new GetProfilePictureQueryResponse { IsSuccess = true, Content = profilePicture.Value };
    }
}