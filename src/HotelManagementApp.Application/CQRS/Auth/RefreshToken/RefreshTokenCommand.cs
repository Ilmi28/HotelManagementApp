using HotelManagementApp.Application.Responses.AuthResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Auth.RefreshToken;

public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
{
    public required string RefreshToken { get; set; }
}
