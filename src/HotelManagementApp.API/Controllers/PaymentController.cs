using HotelManagementApp.Application.CQRS.PaymentOps.GetAllPayments;
using HotelManagementApp.Application.CQRS.PaymentOps.GetCashPaymentByPayment;
using HotelManagementApp.Application.CQRS.PaymentOps.GetCashPayments;
using HotelManagementApp.Application.CQRS.PaymentOps.GetCreditCardPaymentByPayment;
using HotelManagementApp.Application.CQRS.PaymentOps.GetCreditCardPayments;
using HotelManagementApp.Application.CQRS.PaymentOps.GetPaymentByOrder;
using HotelManagementApp.Application.CQRS.PaymentOps.GetPaymentMethods;
using HotelManagementApp.Application.CQRS.PaymentOps.PayWithCash;
using HotelManagementApp.Application.CQRS.PaymentOps.PayWithCreditCard;
using HotelManagementApp.Application.Events.OrderCompleted;
using HotelManagementApp.Application.Responses.OrderResponses;
using HotelManagementApp.Application.Responses.PaymentResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[ApiController]
[Authorize]
[Route("api/payment")]
public class PaymentController(IMediator mediator) : ControllerBase
{
    [HttpGet("payment-methods")]
    public async Task<ActionResult<ICollection<PaymentMethodResponse>>> GetPaymentMethods(CancellationToken cancellationToken)
    {
        var query = new GetPaymentMethodsQuery();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("pay-with-cash")]
    public async Task<IActionResult> PayWithCash([FromBody] PayWithCashCommand cmd, 
        IAuthorizationService authService,  CancellationToken cancellationToken)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, cmd.OrderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, cancellationToken);
        await mediator.Publish(new OrderCompletedEvent { OrderId = cmd.OrderId }, cancellationToken);
        return NoContent();
    }

    [HttpPost("pay-with-credit-card")]
    public async Task<IActionResult> PayWithCreditCard([FromBody] PayWithCreditCardCommand cmd,
        IAuthorizationService authService, CancellationToken cancellationToken)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, cmd.OrderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, cancellationToken);
        await mediator.Publish(new OrderCompletedEvent { OrderId = cmd.OrderId }, cancellationToken);
        return NoContent();
    }

    [HttpGet("get-all")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    public async Task<IActionResult> GetAllPayments(CancellationToken cancellationToken)
    {
        var payments = await mediator.Send(new GetAllPaymentsQuery(), cancellationToken);
        return Ok(payments);
    }

    [HttpGet("get-cash-payments")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    public async Task<IActionResult> GetCashPayments(CancellationToken cancellationToken)
    {
        var payments = await mediator.Send(new GetCashPaymentsQuery(), cancellationToken);
        return Ok(payments);
    }

    [HttpGet("get-credit-card-payments")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    public async Task<IActionResult> GetCreditCardPayments(CancellationToken cancellationToken)
    {
        var payments = await mediator.Send(new GetCreditCardPaymentsQuery(), cancellationToken);
        return Ok(payments);
    }

    [HttpGet("get-credit-card-payment/{paymentId}")]
    public async Task<IActionResult> GetCreditCardPaymentByPaymentId(int paymentId, IAuthorizationService authService, 
        CancellationToken cancellationToken)
    {
        var paymentPolicy = await authService.AuthorizeAsync(User, paymentId, "PaymentAccess");
        if (!paymentPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetCreditCardPaymentByPaymentIdQuery {PaymentId = paymentId}, cancellationToken);
        return Ok(response);
    }

    [HttpGet("get-cash-payment/{paymentId}")]
    public async Task<IActionResult> GetCashPaymentByPaymentId(int paymentId, IAuthorizationService authService,
        CancellationToken cancellationToken)
    {
        var paymentPolicy = await authService.AuthorizeAsync(User, paymentId, "PaymentAccess");
        if (!paymentPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetCashPaymentByPaymentIdQuery {PaymentId = paymentId}, cancellationToken);
        return Ok(response);
    }

    [HttpGet("get-payment-by-order/{orderId}")]
    public async Task<IActionResult> GetPaymentByOrderId(int orderId, IAuthorizationService authService,
        CancellationToken cancellationToken)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, orderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetPaymentByOrderIdQuery {OrderId = orderId}, cancellationToken);
        return Ok(response);
    }
}