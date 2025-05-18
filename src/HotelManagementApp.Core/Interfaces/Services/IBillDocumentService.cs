using HotelManagementApp.Core.Models.OrderModels;

namespace HotelManagementApp.Core.Interfaces.Services;

public interface IBillDocumentService
{
    Task<byte[]> GenerateBillDocument(Order order, CancellationToken ct);
}