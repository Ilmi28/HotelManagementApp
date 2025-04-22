using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Hotel.GetById;

public class GetHotelByIdQueryHandler(IHotelRepository hotelRepository) : IRequestHandler<GetHotelByIdQuery, HotelResponse>
{
    public async Task<HotelResponse> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        return new HotelResponse
        {
            Id = hotel.Id,
            Name = hotel.Name,
            Address = hotel.Address,
            City = hotel.City,
            Country = hotel.Country,
            Description = hotel.Description,
            PhoneNumber = hotel.PhoneNumber,
            Email = hotel.Email
        };
    }
}
