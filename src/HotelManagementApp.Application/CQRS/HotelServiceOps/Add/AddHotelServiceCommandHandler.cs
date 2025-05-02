using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelServiceOps.Add;

public class AddHotelServiceCommandHandler(
    IHotelRepository hotelRepository,
    IHotelServiceRepository hotelServiceRepository) : IRequestHandler<AddHotelServiceCommand>
{
    public async Task Handle(AddHotelServiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var hotelService = new HotelService
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Hotel = hotel
        };
        await hotelServiceRepository.AddHotelService(hotelService, cancellationToken);
    }
}
