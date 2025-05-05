using HotelManagementApp.Core.Models.DiscountModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.DiscountRepositories;

public interface IDiscountRepository<T>
{
    Task AddDiscount(T discount, CancellationToken ct = default);
    Task<T?> GetDiscountById(int id, CancellationToken ct = default);
    Task<ICollection<T>> GetAllDiscounts(CancellationToken ct = default);
    Task RemoveDiscount(T discount, CancellationToken ct = default);
    Task<ICollection<T>> GetDiscountsByTypeId(int id, CancellationToken ct = default);
}
