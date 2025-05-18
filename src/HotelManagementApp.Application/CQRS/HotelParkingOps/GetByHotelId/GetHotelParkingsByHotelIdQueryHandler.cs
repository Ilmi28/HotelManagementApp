using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelParkingOps.GetByHotelId;

public class GetHotelParkingsByHotelIdQueryHandler(
    IHotelRepository hotelRepository,
    IHotelParkingRepository parkingRepository,
    IPricingService pricingService) 
    : IRequestHandler<GetHotelParkingsByHotelIdQuery, ICollection<HotelParkingResponse>>
{
    public async Task<ICollection<HotelParkingResponse>> Handle(GetHotelParkingsByHotelIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var parkings = await parkingRepository.GetHotelParkingsByHotelId(hotel.Id, cancellationToken);
        var response = new List<HotelParkingResponse>();
        foreach (var parking in parkings)
        {
            var finalPrice = await pricingService.CalculatePriceForParking(parking, cancellationToken);
            response.Add(new HotelParkingResponse
            {
                Id = parking.Id,
                CarSpaces = parking.CarSpaces,
                Description = parking.Description,
                Price = parking.Price,
                HotelId = parking.Hotel.Id,
                DiscountPercent = 100 - (100 * finalPrice / parking.Price),
                FinalPrice = finalPrice,
            });
        }

        return response;
    }
}
