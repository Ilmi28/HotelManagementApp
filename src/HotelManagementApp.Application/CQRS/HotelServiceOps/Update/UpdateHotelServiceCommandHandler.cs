using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelServiceOps.Update;

public class UpdateHotelServiceCommandHandler(
    IHotelRepository hotelRepository, 
    IHotelServiceRepository hotelServiceRepository) 
    : IRequestHandler<UpdateHotelServiceCommand>
{
    public async Task Handle(UpdateHotelServiceCommand request, CancellationToken cancellationToken)
    {
        var hotelService = await hotelServiceRepository.GetHotelServiceById(request.Id, cancellationToken)
            ?? throw new HotelServiceNotFoundException($"Hotel service with id {request.Id} not found");
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        hotelService.Name = request.Name;
        hotelService.Description = request.Description;
        hotelService.Price = request.Price;
        hotelService.Hotel = hotel;
        await hotelServiceRepository.UpdateHotelService(hotelService, cancellationToken);
    }
}
