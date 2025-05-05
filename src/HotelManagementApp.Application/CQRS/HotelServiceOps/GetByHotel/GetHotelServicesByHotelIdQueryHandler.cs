using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelServiceOps.GetByHotel;

public class GetHotelServicesByHotelIdQueryHandler(
    IHotelRepository hotelRepository, 
    IHotelServiceRepository hotelServiceRepository,
    IHotelDiscountRepository hotelDiscountRepository,
    IServiceDiscountRepository serviceDiscountRepository) 
    : IRequestHandler<GetHotelServicesByHotelIdQuery, ICollection<HotelServiceResponse>>
{
    public async Task<ICollection<HotelServiceResponse>> Handle(GetHotelServicesByHotelIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with id {request.HotelId} not found");
        var hotelServices = await hotelServiceRepository.GetHotelServicesByHotel(hotel.Id, cancellationToken);
        var hotelDiscounts = await hotelDiscountRepository.GetDiscountsByTypeId(hotel.Id, cancellationToken);
        var serviceDiscounts = await serviceDiscountRepository.GetDiscountsByTypeId(hotel.Id, cancellationToken);
        int totalDiscountPercent = 0;
        foreach (var discount in hotelDiscounts)
        {
            if (discount.From < DateTime.Now && discount.To > DateTime.Now)
                totalDiscountPercent += discount.DiscountPercent;
        }
        foreach (var discount in serviceDiscounts)
        {
            if (discount.From < DateTime.Now && discount.To > DateTime.Now)
                totalDiscountPercent += discount.DiscountPercent;
        }
        var response = hotelServices.Select(h => new HotelServiceResponse
        {
            Id = h.Id,
            Name = h.Name,
            Description = h.Description,
            Price = h.Price,
            HotelId = h.Hotel.Id,
            Discount = totalDiscountPercent <= 100 ? totalDiscountPercent : 100,
        }).ToList();
        return response;
    }
}
