using HotelManagementApp.Core.Dtos;
using MediatR;

namespace HotelManagementApp.Application.CQRS.VIP.GetAll;

public class GetVIPListQuery : IRequest<ICollection<UserDto>>
{

}
