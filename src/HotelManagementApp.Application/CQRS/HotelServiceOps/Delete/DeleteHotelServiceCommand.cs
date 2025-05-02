using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelServiceOps.Delete;

public class DeleteHotelServiceCommand : IRequest
{
    public required int HotelServiceId { get; set; }
}
