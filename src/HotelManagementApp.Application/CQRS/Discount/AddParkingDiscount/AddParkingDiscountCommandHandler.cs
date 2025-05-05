using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.AddParkingDiscount;

public class AddParkingDiscountCommandHandler(
    IHotelParkingRepository parkingRepository,
    IParkingDiscountRepository discountRepository) : IRequestHandler<AddParkingDiscountCommand>
{
    public async Task Handle(AddParkingDiscountCommand request, CancellationToken cancellationToken)
    {
        var parking = await parkingRepository.GetHotelParkingById(request.ParkingId, cancellationToken)
            ?? throw new HotelParkingNotFoundException($"Hotel parking with id {request.ParkingId} not found");
        var discount = new ParkingDiscount
        {
            Parking = parking,
            DiscountPercent = request.DiscountPercent,
            From = request.From,
            To = request.To
        };
        await discountRepository.AddDiscount(discount, cancellationToken);
    }
}
