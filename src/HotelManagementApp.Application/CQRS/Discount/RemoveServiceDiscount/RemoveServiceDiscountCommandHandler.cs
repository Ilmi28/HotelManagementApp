using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using MediatR;

namespace HotelManagementApp.Application.CQRS.Discount.RemoveServiceDiscount;

public class RemoveServiceDiscountCommandHandler(IServiceDiscountRepository discountRepository) : IRequestHandler<RemoveServiceDiscountCommand>
{
    public async Task Handle(RemoveServiceDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await discountRepository.GetDiscountById(request.DiscountId, cancellationToken)
            ?? throw new DiscountNotFoundException($"Discount with ID {request.DiscountId} not found.");
        await discountRepository.RemoveDiscount(discount, cancellationToken);
    }
}
