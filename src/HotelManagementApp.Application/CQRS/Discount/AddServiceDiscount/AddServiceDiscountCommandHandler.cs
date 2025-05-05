using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.HotelRepositories;
using HotelManagementApp.Core.Models.DiscountModels;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.AddServiceDiscount;

public class AddServiceDiscountCommandHandler(
    IHotelServiceRepository serviceRepository, 
    IServiceDiscountRepository discountRepository) : IRequestHandler<AddServiceDiscountCommand>
{
    public async Task Handle(AddServiceDiscountCommand request, CancellationToken cancellationToken)
    {
        var service = await serviceRepository.GetHotelServiceById(request.ServiceId, cancellationToken)
            ?? throw new HotelServiceNotFoundException($"Service with id {request.ServiceId} not found");
        var discount = new ServiceDiscount
        {
            Service = service,
            DiscountPercent = request.DiscountPercent,
            From = request.From,
            To = request.To
        };
        await discountRepository.AddDiscount(discount, cancellationToken);
    }
}
