using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.History;

public class GetAccountHistoryQuery : IRequest<ICollection<AccountLogResponse>>
{
    public required string UserId { get; set; }
}
