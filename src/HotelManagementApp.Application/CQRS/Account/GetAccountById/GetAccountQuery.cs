using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Account.GetAccountById
{
    public class GetAccountQuery : IRequest<AccountResponse>
    {
        public required string UserId { get; set; }
    }
}
