using HotelManagementApp.Application.Responses.DiscountResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.GetDiscountsByParking;

public class GetDiscountsByParkingQueryHandler(
    IHotelParkingRepository parkingRepository,
    IParkingDiscountRepository discountRepository) : IRequestHandler<GetDiscountsByParkingQuery, ICollection<ParkingDiscountResponse>>
{
    public async Task<ICollection<ParkingDiscountResponse>> Handle(GetDiscountsByParkingQuery request, CancellationToken cancellationToken)
    {
        var parking = await parkingRepository.GetHotelParkingById(request.ParkingId, cancellationToken)
            ?? throw new HotelParkingNotFoundException($"Hotel parking with id {request.ParkingId} not found");
        var discounts = await discountRepository.GetDiscountsByTypeId(parking.Id, cancellationToken);
        return discounts.Select(x => new ParkingDiscountResponse
        {
            Id = x.Id,
            ParkingId = x.Parking.Id,
            From = x.From,
            To = x.To,
            DiscountPercent = x.DiscountPercent,
        }).ToList();
    }
}
