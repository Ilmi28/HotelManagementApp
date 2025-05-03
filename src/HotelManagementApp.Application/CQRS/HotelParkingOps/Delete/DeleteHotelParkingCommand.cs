using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelParkingOps.Delete;

public class DeleteHotelParkingCommand : IRequest
{
    public required int Id { get; set; }
}
