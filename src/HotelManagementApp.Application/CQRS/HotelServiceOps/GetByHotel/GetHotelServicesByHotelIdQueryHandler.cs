using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelServiceOps.GetByHotel;

public class GetHotelServicesByHotelIdQueryHandler(
    IHotelRepository hotelRepository, 
    IHotelServiceRepository hotelServiceRepository) 
    : IRequestHandler<GetHotelServicesByHotelIdQuery, ICollection<HotelServiceResponse>>
{
    public async Task<ICollection<HotelServiceResponse>> Handle(GetHotelServicesByHotelIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var hotelServices = await hotelServiceRepository.GetHotelServicesByHotel(hotel.Id, cancellationToken);
        var response = hotelServices.Select(h => new HotelServiceResponse
        {
            Id = h.Id,
            Name = h.Name,
            Description = h.Description,
            Price = h.Price,
            HotelId = h.Hotel.Id
        }).ToList();
        return response;
    }
}
