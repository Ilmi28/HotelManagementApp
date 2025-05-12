using HotelManagementApp.Core.Models.PaymentModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;

public interface IPaymentRepository
{
    Task<Payment?> GetPaymentById(int id, CancellationToken ct = default);
    Task<Payment?> GetPaymentsByOrderId(int orderId, CancellationToken ct = default);
    Task<ICollection<Payment>> GetPayments(CancellationToken ct = default);
    Task AddPayment(Payment payment, CancellationToken ct = default);
    Task<ICollection<Payment>> GetPaymentsByUserId(string guestId, CancellationToken ct = default);
    Task<ICollection<PaymentMethod>> GetPaymentMethods(CancellationToken ct = default);
}