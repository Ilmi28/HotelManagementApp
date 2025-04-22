using HotelManagementApp.Core.Interfaces.Repositories;
using HotelManagementApp.Core.Models.HotelModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Hotel.Add;

public class AddHotelCommandHandler(IHotelRepository hotelRepository) : IRequestHandler<AddHotelCommand>
{
    public async Task Handle(AddHotelCommand request, CancellationToken cancellationToken)
    {
        var hotelModel = new HotelModel
        {
            Name = request.Name,
            Address = request.Address,
            City = request.City,
            Country = request.Country,
            Description = request.Description,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email
        };

        await hotelRepository.AddHotel(hotelModel, cancellationToken);
    }
}
