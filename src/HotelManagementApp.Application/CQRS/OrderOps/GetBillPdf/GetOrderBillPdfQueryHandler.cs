using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Exceptions.NotFound;
using HotelManagementApp.Core.Interfaces.Repositories.OrderRepositories;
using HotelManagementApp.Core.Interfaces.Services;
using MediatR;

namespace HotelManagementApp.Application.CQRS.OrderOps.GetBillPdf;

public class GetOrderBillPdfQueryHandler(
    IBillDocumentService billService, 
    IOrderRepository orderRepository) : IRequestHandler<GetOrderBillPdfQuery, byte[]>
{
    public async Task<byte[]> Handle(GetOrderBillPdfQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetOrderById(request.OrderId, cancellationToken) ??
            throw new OrderNotFoundException($"Order with id {request.OrderId} not found");
        if (order.Status != OrderStatusEnum.Completed)
            throw new InvalidOperationException($"Order status should be completed. Current status of order {order.Id}: {order.Status}");
        return await billService.GenerateBillDocument(order!, cancellationToken);
    }
}