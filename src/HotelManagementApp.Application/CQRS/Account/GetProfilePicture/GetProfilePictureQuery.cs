using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.GetProfilePicture;

public class GetProfilePictureQuery : IRequest<byte[]>
{
    public required string UserId { get; set; } 
}
