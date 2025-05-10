using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelServiceOps.GetByHotel;

public class GetHotelServicesByHotelIdQueryHandler(
    IHotelRepository hotelRepository, 
    IHotelServiceRepository hotelServiceRepository,
    IServiceDiscountService discountService) 
    : IRequestHandler<GetHotelServicesByHotelIdQuery, ICollection<HotelServiceResponse>>
{
    public async Task<ICollection<HotelServiceResponse>> Handle(GetHotelServicesByHotelIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var hotelServices = await hotelServiceRepository.GetHotelServicesByHotel(hotel.Id, cancellationToken);
        var response = new List<HotelServiceResponse>();
        foreach (var hotelService in hotelServices)
        {
            response.Add(new HotelServiceResponse
            {
                Id = hotelService.Id,
                Name = hotelService.Name,
                Description = hotelService.Description,
                Price = hotelService.Price,
                HotelId = hotelService.Hotel.Id,
                Discount = await discountService.CalculateDiscount(hotelService, cancellationToken),
            });
        }
        return response;
    }
}
