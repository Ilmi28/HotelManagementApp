using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelParkingOps.GetByHotelId;

public class GetHotelParkingsByHotelIdQueryHandler(
    IHotelRepository hotelRepository,
    IHotelParkingRepository parkingRepository,
    IHotelDiscountRepository hotelDiscountRepository,
    IParkingDiscountRepository parkingDiscountRepository) 
    : IRequestHandler<GetHotelParkingsByHotelIdQuery, ICollection<HotelParkingResponse>>
{
    public async Task<ICollection<HotelParkingResponse>> Handle(GetHotelParkingsByHotelIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var parkings = await parkingRepository.GetHotelParkingsByHotelId(hotel.Id, cancellationToken);
        var hotelDiscounts = await hotelDiscountRepository.GetDiscountsByTypeId(hotel.Id, cancellationToken);
        var parkingDiscounts = await parkingDiscountRepository.GetDiscountsByTypeId(hotel.Id, cancellationToken);
        int totalDiscountPercent = 0;
        foreach (var hotelDiscount in hotelDiscounts)
        {
            if (hotelDiscount.From < DateTime.Now && hotelDiscount.To > DateTime.Now)
                totalDiscountPercent += hotelDiscount.DiscountPercent;
        }
        foreach (var parkingDiscount in parkingDiscounts)
        {
            if (parkingDiscount.From < DateTime.Now && parkingDiscount.To > DateTime.Now)
                totalDiscountPercent += parkingDiscount.DiscountPercent;
        }
        return parkings.Select(p => new HotelParkingResponse
        {
            Id = p.Id,
            CarSpaces = p.CarSpaces,
            Description = p.Description,
            Price = p.Price,
            HotelId = p.Hotel.Id,
            DiscountPercent = totalDiscountPercent <= 100 ? totalDiscountPercent : 100,
        }).ToList();
    }
}
