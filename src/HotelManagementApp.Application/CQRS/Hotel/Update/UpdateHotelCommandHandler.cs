using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Hotel.Update;

public class UpdateHotelCommandHandler(IHotelRepository hotelRepository) : IRequestHandler<UpdateHotelCommand>
{
    public async Task Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotelModel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        hotelModel.Name = request.Name;
        hotelModel.Address = request.Address;
        hotelModel.City = request.City;
        hotelModel.Country = request.Country;
        hotelModel.Description = request.Description;
        hotelModel.PhoneNumber = request.PhoneNumber;
        hotelModel.Email = request.Email;
        await hotelRepository.UpdateHotel(hotelModel, cancellationToken);
    }
}
