using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Models.PaymentModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.PaymentRepositories;

public class CreditCardPaymentRepository(AppDbContext context) : ICreditCardPaymentRepository
{
    public async Task AddCreditCardPayment(CreditCardPayment creditCardPayment, CancellationToken ct)
    {
        context.Attach(creditCardPayment.Payment);
        await context.CreditCardPayments.AddAsync(creditCardPayment, ct);
        await context.SaveChangesAsync(ct);
    }
    
    public async Task<ICollection<CreditCardPayment>> GetCreditCardPayments(CancellationToken ct)
    {
        return await context.CreditCardPayments
                .AsNoTracking()
            .Include(x => x.Payment)
            .ToListAsync(ct);
    }
    
    public async Task<CreditCardPayment?> GetCreditCardPaymentById(int id, CancellationToken ct)
    {
        return await context.CreditCardPayments
                .AsNoTracking()
            .Include(x => x.Payment)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<CreditCardPayment?> GetCreditCardPaymentByPaymentId(int paymentId, CancellationToken ct)
    {
        return await context.CreditCardPayments
                .AsNoTracking()
                .Include(x => x.Payment)
                .FirstOrDefaultAsync(x => x.Payment.Id == paymentId, ct);
    }
}