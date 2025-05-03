using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelOps.GetAll;

public class GetAllHotelsQuery : IRequest<ICollection<HotelResponse>>
{

}
