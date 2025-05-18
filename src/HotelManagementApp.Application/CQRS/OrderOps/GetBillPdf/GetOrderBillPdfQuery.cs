using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetBillPdf;

public class GetOrderBillPdfQuery : IRequest<byte[]>
{
    public int OrderId { get; set; }
}