using HotelManagementApp.Application.Responses.DiscountResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.GetDiscountsByService;

public class GetDiscountsByServiceQueryHandler(
    IHotelServiceRepository serviceRepository, 
    IServiceDiscountRepository discountRepository) : IRequestHandler<GetDiscountsByServiceQuery, ICollection<ServiceDiscountResponse>>
{
    public async Task<ICollection<ServiceDiscountResponse>> Handle(GetDiscountsByServiceQuery request, CancellationToken cancellationToken)
    {
        var service = await serviceRepository.GetHotelServiceById(request.ServiceId, cancellationToken)
            ?? throw new HotelServiceNotFoundException($"Hotel service with id {request.ServiceId} not found");
        var discounts = await discountRepository.GetDiscountsByTypeId(request.ServiceId, cancellationToken);
        return discounts
            .Select(x => new ServiceDiscountResponse
            {
                Id = x.Id,
                ServiceId = x.Service.Id,
                DiscountPercent = x.DiscountPercent,
                From = x.From,
                To = x.To
            })
            .ToList();
    }
}
