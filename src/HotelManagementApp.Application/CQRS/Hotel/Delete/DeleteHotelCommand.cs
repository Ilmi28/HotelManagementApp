using MediatR;

namespace HotelManagementApp.Application.CQRS.Hotel.Delete;

public class DeleteHotelCommand : IRequest
{
    public int HotelId { get; set; }
}
