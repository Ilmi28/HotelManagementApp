using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelParkingOps.GetByHotelId;

public class GetHotelParkingsByHotelIdQueryHandler(
    IHotelRepository hotelRepository,
    IHotelParkingRepository parkingRepository) : IRequestHandler<GetHotelParkingsByHotelIdQuery, ICollection<HotelParkingResponse>>
{
    public async Task<ICollection<HotelParkingResponse>> Handle(GetHotelParkingsByHotelIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var parkings = await parkingRepository.GetHotelParkingsByHotelId(hotel.Id, cancellationToken);
        return parkings.Select(p => new HotelParkingResponse
        {
            Id = p.Id,
            CarSpaces = p.CarSpaces,
            Description = p.Description,
            Price = p.Price,
            HotelId = p.Hotel.Id,
        }).ToList();
    }
}
