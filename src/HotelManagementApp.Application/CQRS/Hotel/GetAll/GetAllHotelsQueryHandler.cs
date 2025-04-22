using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Interfaces.Repositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Hotel.GetAll;

public class GetAllHotelsQueryHandler(IHotelRepository hotelRepository) : IRequestHandler<GetAllHotelsQuery, ICollection<HotelResponse>>
{
    public async Task<ICollection<HotelResponse>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
    {
        var hotelModels = await hotelRepository.GetAllHotels(cancellationToken);
        return hotelModels.Select(h => new HotelResponse
        {
            Id = h.Id,
            Name = h.Name,
            Address = h.Address,
            City = h.City,
            Country = h.Country,
            Description = h.Description,
            PhoneNumber = h.PhoneNumber,
            Email = h.Email
        }).ToList();
    }
}
