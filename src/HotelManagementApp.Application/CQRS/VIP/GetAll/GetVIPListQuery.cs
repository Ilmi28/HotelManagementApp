using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.VIP.GetAll;

public class GetVIPListQuery : IRequest<ICollection<AccountResponse>>
{

}
