using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelServiceOps.GetById;

public class GetHotelServiceByIdQuery : IRequest<HotelServiceResponse>
{
    public required int ServiceId { get; set; }
}