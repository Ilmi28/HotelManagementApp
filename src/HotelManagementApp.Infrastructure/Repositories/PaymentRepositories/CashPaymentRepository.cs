using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Models.PaymentModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.PaymentRepositories;

public class CashPaymentRepository(AppDbContext context) : ICashPaymentRepository
{
    public async Task AddCashPayment(CashPayment cashPayment, CancellationToken ct)
    {
        context.Attach(cashPayment.Payment);
        await context.CashPayments.AddAsync(cashPayment, ct);
        await context.SaveChangesAsync(ct);
    }
    
    public async Task<ICollection<CashPayment>> GetCashPayments(CancellationToken ct)
    {
        return await context.CashPayments
                .AsNoTracking()
            .Include(x => x.Payment)
            .ToListAsync(ct);
    }
    
    public async Task<CashPayment?> GetCashPaymentById(int id, CancellationToken ct)
    {
        return await context.CashPayments
                .AsNoTracking()
            .Include(x => x.Payment)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<CashPayment?> GetCashPaymentByPaymentId(int paymentId, CancellationToken ct)
    {
        return await context.CashPayments
                .AsNoTracking()
                .Include(x => x.Payment)
                .FirstOrDefaultAsync(x => x.Payment.Id == paymentId, ct);
    }
}