using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Contracts.Queries;
using HomeHive.Application.Persistence;
using Microsoft.Extensions.Options;

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
        
        var blobName = user.Value.ProfilePicture ?? "default-profile-photo.jpg";

        var profilePicture = await blobStorageService.GetBlobAsync(blobName, cancellationToken);

        return !profilePicture.IsSuccess
            ? new GetProfilePictureQueryResponse { IsSuccess = false, Message = "Profile picture not found" }
            : new GetProfilePictureQueryResponse { IsSuccess = true, Content = profilePicture.Value };
    }
}