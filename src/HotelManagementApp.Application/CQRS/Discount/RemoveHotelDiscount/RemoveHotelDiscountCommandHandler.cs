using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.RemoveHotelDiscount;

public class RemoveHotelDiscountCommandHandler(IHotelDiscountRepository hotelDiscountRepository) : IRequestHandler<RemoveHotelDiscountCommand>
{
    public async Task Handle(RemoveHotelDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await hotelDiscountRepository.GetDiscountById(request.DiscountId, cancellationToken)
            ?? throw new DiscountNotFoundException($"Discount with ID {request.DiscountId} not found.");
        await hotelDiscountRepository.RemoveDiscount(discount, cancellationToken);
    }
}
