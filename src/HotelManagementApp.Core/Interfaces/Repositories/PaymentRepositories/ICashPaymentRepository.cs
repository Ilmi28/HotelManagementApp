using HotelManagementApp.Core.Models.PaymentModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;

public interface ICashPaymentRepository
{
    Task AddCashPayment(CashPayment cashPayment, CancellationToken ct);
    Task<ICollection<CashPayment>> GetCashPayments(CancellationToken ct);
    Task<CashPayment?> GetCashPaymentById(int id, CancellationToken ct);
    Task<CashPayment?> GetCashPaymentByPaymentId(int paymentId, CancellationToken ct);
}