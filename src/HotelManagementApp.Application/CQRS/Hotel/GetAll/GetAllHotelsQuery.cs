using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Hotel.GetAll;

public class GetAllHotelsQuery : IRequest<ICollection<HotelResponse>>
{

}
