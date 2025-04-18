using MediatR;

namespace HotelManagementApp.Application.CQRS.UserProfilePicture.GetProfilePicture;

public class GetProfilePictureQuery : IRequest<byte[]>
{
    public required string UserId { get; set; } 
}
