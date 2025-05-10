namespace HotelManagementApp.Core.Interfaces.Services;

public interface IDiscountService<T>
{
    Task<int> CalculateDiscount(T model, CancellationToken ct);
}