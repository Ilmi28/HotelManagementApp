using HotelManagementApp.Core.Dtos;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Blacklist.GetAll;

public class GetBlacklistQuery : IRequest<ICollection<UserDto>>
{

}
