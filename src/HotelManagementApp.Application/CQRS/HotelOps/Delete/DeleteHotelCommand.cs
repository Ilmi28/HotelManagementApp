using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelOps.Delete;

public class DeleteHotelCommand : IRequest
{
    public int HotelId { get; set; }
}
