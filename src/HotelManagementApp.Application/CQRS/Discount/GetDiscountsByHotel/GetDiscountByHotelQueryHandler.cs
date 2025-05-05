using HotelManagementApp.Application.Responses.DiscountResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.GetDiscountsByHotel;

public class GetDiscountByHotelQueryHandler(
    IHotelRepository hotelRepository, 
    IHotelDiscountRepository discountRepository) : IRequestHandler<GetDiscountByHotelQuery, ICollection<HotelDiscountResponse>>
{
    public async Task<ICollection<HotelDiscountResponse>> Handle(GetDiscountByHotelQuery request, CancellationToken cancellationToken)
    {
        var hotel = hotelRepository.GetHotelById(request.HotelId, cancellationToken)
            ?? throw new HotelNotFoundException($"Hotel with if {request.HotelId} not found");
        var discounts = await discountRepository.GetDiscountsByTypeId(request.HotelId, cancellationToken);
        return discounts.Select(x => new HotelDiscountResponse
        {
            Id = x.Id,
            HotelId = x.Hotel.Id,
            DiscountPercent = x.DiscountPercent,
            From = x.From,
            To = x.To,
        }).ToList();
    }
}
