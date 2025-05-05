using HotelManagementApp.Application.CQRS.HotelOps.Add;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.AddHotelDiscount;

public class AddHotelDiscountCommandHandler(
    IHotelRepository hotelRepository, 
    IHotelDiscountRepository hotelDiscountRepository) : IRequestHandler<AddHotelDiscountCommand>
{
    public async Task Handle(AddHotelDiscountCommand request, CancellationToken cancellationToken)
    {
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var hotelDiscount = new HotelDiscount
        {
            Hotel = hotel,
            DiscountPercent = request.DiscountPercent,
            From = request.From,
            To = request.To
        };
        await hotelDiscountRepository.AddDiscount(hotelDiscount, cancellationToken);
    }
}
