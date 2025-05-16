using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Application.Services.DiscountServices;

public class ServiceDiscountService(
    IHotelDiscountRepository hotelDiscountRepository, 
    IServiceDiscountRepository serviceDiscountRepository) : IServiceDiscountService
{
    public async Task<int> CalculateDiscount(HotelService model, CancellationToken ct)
    {
        var serviceDiscounts = await serviceDiscountRepository.GetDiscountsByTypeId(model.Id, ct);
        var hotelDiscounts = await hotelDiscountRepository.GetDiscountsByTypeId(model.Hotel.Id, ct);
        int totalDiscountPercent = 0;
        foreach (var discount in serviceDiscounts)
        {
            if (discount.From < DateTime.Now && discount.To > DateTime.Now)
                totalDiscountPercent += discount.DiscountPercent;
        }
        foreach (var discount in hotelDiscounts)
        {
            if (discount.From < DateTime.Now && discount.To > DateTime.Now)
                totalDiscountPercent += discount.DiscountPercent;
        }
        return totalDiscountPercent > 100 ? 100 : totalDiscountPercent;
    }
}