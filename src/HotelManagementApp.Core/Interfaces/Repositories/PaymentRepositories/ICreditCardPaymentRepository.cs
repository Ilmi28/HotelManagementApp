using HotelManagementApp.Core.Models.PaymentModels;

namespace HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;

public interface ICreditCardPaymentRepository
{
    Task AddCreditCardPayment(CreditCardPayment creditCardPayment, CancellationToken ct);
    Task<ICollection<CreditCardPayment>> GetCreditCardPayments(CancellationToken ct);
    Task<CreditCardPayment?> GetCreditCardPaymentById(int id, CancellationToken ct);
    Task<CreditCardPayment?> GetCreditCardPaymentByPaymentId(int paymentId, CancellationToken ct);
}