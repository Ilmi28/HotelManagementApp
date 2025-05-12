using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Repositories.PaymentRepositories;
using HotelManagementApp.Core.Models.PaymentModels;
using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Repositories.PaymentRepositories;

public class PaymentRepository(AppDbContext context) : IPaymentRepository
{
    public async Task<Payment?> GetPaymentById(int id, CancellationToken ct)
    {
        return await context.Payments
            .AsNoTracking()
            .Include(x => x.Order)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
    
    public async Task<Payment?> GetPaymentsByOrderId(int orderId, CancellationToken ct)
    {
        return await context.Payments
            .AsNoTracking()
            .Include(x => x.Order)
            .FirstOrDefaultAsync(x => x.Order.Id == orderId, ct);
    }
    
    public async Task<ICollection<Payment>> GetPayments(CancellationToken ct)
    {
        return await context.Payments
            .AsNoTracking()
            .Include(x => x.Order)
            .ToListAsync(ct);
    }
    
    public async Task AddPayment(Payment payment, CancellationToken ct)
    {
        context.Attach(payment.Order);
        await context.Payments.AddAsync(payment, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<ICollection<Payment>> GetPaymentsByUserId(string guestId, CancellationToken ct)
    {
        return await context.Payments
            .AsNoTracking()
            .Include(x => x.Order)
            .Where(x => x.Order.UserId == guestId)
            .ToListAsync(ct);
    }

    public async Task<ICollection<PaymentMethod>> GetPaymentMethods(CancellationToken ct)
    {
        return await context.PaymentMethods
            .AsNoTracking()
            .ToListAsync(ct);
    }
}