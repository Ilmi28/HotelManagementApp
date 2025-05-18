using HotelManagementApp.Application.Responses.HotelResponses;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.HotelServiceOps.GetById;

public class GetHotelServiceByIdQueryHandler(
    IHotelServiceRepository serviceRepository,
    IPricingService pricingService)
    : IRequestHandler<GetHotelServiceByIdQuery, HotelServiceResponse>
{
    public async Task<HotelServiceResponse> Handle(GetHotelServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var service = await serviceRepository.GetHotelServiceById(request.ServiceId, cancellationToken)
            ?? throw new HotelServiceNotFoundException($"Hotel service with id {request.ServiceId} not found");
            
        var finalPrice = await pricingService.CalculatePriceForService(service, cancellationToken);
        
        return new HotelServiceResponse
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description,
            Price = service.Price,
            HotelId = service.Hotel.Id,
            Discount = 100 - (100 * finalPrice / service.Price),
            FinalPrice = finalPrice
        };
    }
}