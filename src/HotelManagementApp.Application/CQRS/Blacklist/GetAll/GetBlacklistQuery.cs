using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Blacklist.GetAll;

public class GetBlacklistQuery : IRequest<ICollection<AccountResponse>>
{

}
