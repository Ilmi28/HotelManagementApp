using HotelManagementApp.Application.Responses.AuthResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Auth.RefreshSession;

public class RefreshSessionCommand : IRequest<RefreshTokenResponse>
{
    public required string RefreshToken { get; set; }
}
