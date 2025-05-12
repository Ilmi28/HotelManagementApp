using HotelManagementApp.Application.Responses.AccountResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.VIP.GetAll;

public class GetVipListQuery : IRequest<ICollection<AccountResponse>>
{

}
