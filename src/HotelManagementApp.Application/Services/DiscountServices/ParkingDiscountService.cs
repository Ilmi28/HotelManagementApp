using HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;

namespace HotelManagementApp.Application.Services.DiscountServices;

public class ParkingDiscountService(
    IHotelDiscountRepository hotelDiscountRepository, 
    IParkingDiscountRepository parkingDiscountRepository) : IParkingDiscountService
{
    public async Task<int> CalculateDiscount(HotelParking model, CancellationToken ct)
    {
        var parkingDiscounts = await parkingDiscountRepository.GetDiscountsByTypeId(model.Id, ct);
        var hotelDiscounts = await hotelDiscountRepository.GetDiscountsByTypeId(model.Hotel.Id, ct);
        int totalDiscountPercent = 0;
        foreach (var discount in parkingDiscounts)
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