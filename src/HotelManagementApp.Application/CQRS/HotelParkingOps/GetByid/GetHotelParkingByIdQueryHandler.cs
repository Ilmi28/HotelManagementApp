using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelParkingOps.GetByid;

public class GetHotelParkingByIdQueryHandler(
    IHotelParkingRepository parkingRepository,
    IParkingDiscountService discountService) 
    : IRequestHandler<GetHotelParkingByIdQuery, HotelParkingResponse>
{
    public async Task<HotelParkingResponse> Handle(GetHotelParkingByIdQuery request, CancellationToken cancellationToken)
    {
        var parking = await parkingRepository.GetHotelParkingById(request.ParkingId, cancellationToken)
            ?? throw new HotelParkingNotFoundException($"Hotel parking with id {request.ParkingId} not found");
            
        var discount = await discountService.CalculateDiscount(parking, cancellationToken);
        
        return new HotelParkingResponse
        {
            Id = parking.Id,
            CarSpaces = parking.CarSpaces,
            Description = parking.Description,
            Price = parking.Price,
            HotelId = parking.Hotel.Id,
            DiscountPercent = discount,
            FinalPrice = parking.Price - ((discount / 100m) * parking.Price),
        };
    }
}