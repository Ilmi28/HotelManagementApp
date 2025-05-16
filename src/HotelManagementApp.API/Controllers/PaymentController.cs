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
using HotelManagementApp.Core.Models.PaymentModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[ApiController]
[Authorize]
[Route("api/payment")]
public class PaymentController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Returns a list of supported payment methods.
    /// </summary>
    /// <response code="200">Returns the list of payment methods</response>
    [HttpGet("payment-methods")]
    [ProducesResponseType(typeof(ICollection<PaymentMethodResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<PaymentMethodResponse>>> GetPaymentMethods(CancellationToken cancellationToken)
    {
        var query = new GetPaymentMethodsQuery();
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Pays for an order using cash.
    /// </summary>
    /// <response code="204">Payment completed successfully</response>
    /// <response code="403">User is not authorized to access the order</response>
    [HttpPost("pay-with-cash")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> PayWithCash([FromBody] PayWithCashCommand cmd, 
        IAuthorizationService authService,  CancellationToken cancellationToken)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, cmd.OrderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, cancellationToken);
        await mediator.Publish(new OrderCompletedEvent { OrderId = cmd.OrderId }, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Pays for an order using a credit card.
    /// </summary>
    /// <response code="204">Payment completed successfully</response>
    /// <response code="403">User is not authorized to access the order</response>
    [HttpPost("pay-with-credit-card")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> PayWithCreditCard([FromBody] PayWithCreditCardCommand cmd,
        IAuthorizationService authService, CancellationToken cancellationToken)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, cmd.OrderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        await mediator.Send(cmd, cancellationToken);
        await mediator.Publish(new OrderCompletedEvent { OrderId = cmd.OrderId }, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Returns all payment records (admin/staff/manager only).
    /// </summary>
    /// <response code="200">Returns list of payments</response>
    [HttpGet("get-all")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(typeof(ICollection<PaymentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPayments(CancellationToken cancellationToken)
    {
        var payments = await mediator.Send(new GetAllPaymentsQuery(), cancellationToken);
        return Ok(payments);
    }

    /// <summary>
    /// Returns all cash payments (admin/staff/manager only).
    /// </summary>
    /// <response code="200">Returns list of cash payments</response>
    [HttpGet("get-cash-payments")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(typeof(ICollection<CashPaymentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCashPayments(CancellationToken cancellationToken)
    {
        var payments = await mediator.Send(new GetCashPaymentsQuery(), cancellationToken);
        return Ok(payments);
    }

    /// <summary>
    /// Returns all credit card payments (admin/staff/manager only).
    /// </summary>
    /// <response code="200">Returns list of credit card payments</response>
    [HttpGet("get-credit-card-payments")]
    [Authorize(Roles = "Admin, Manager, Staff")]
    [ProducesResponseType(typeof(ICollection<CreditCardPaymentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCreditCardPayments(CancellationToken cancellationToken)
    {
        var payments = await mediator.Send(new GetCreditCardPaymentsQuery(), cancellationToken);
        return Ok(payments);
    }

    /// <summary>
    /// Returns a credit card payment by payment ID.
    /// </summary>
    /// <response code="200">Returns credit card payment details</response>
    /// <response code="403">User is not authorized to access this payment</response>
    [HttpGet("get-credit-card-payment/{paymentId}")]
    [ProducesResponseType(typeof(CreditCardPaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCreditCardPaymentByPaymentId(int paymentId, IAuthorizationService authService, 
        CancellationToken cancellationToken)
    {
        var paymentPolicy = await authService.AuthorizeAsync(User, paymentId, "PaymentAccess");
        if (!paymentPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetCreditCardPaymentByPaymentIdQuery {PaymentId = paymentId}, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Returns a cash payment by payment ID.
    /// </summary>
    /// <response code="200">Returns cash payment details</response>
    /// <response code="403">User is not authorized to access this payment</response>
    [HttpGet("get-cash-payment/{paymentId}")]
    [ProducesResponseType(typeof(CashPaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCashPaymentByPaymentId(int paymentId, IAuthorizationService authService,
        CancellationToken cancellationToken)
    {
        var paymentPolicy = await authService.AuthorizeAsync(User, paymentId, "PaymentAccess");
        if (!paymentPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetCashPaymentByPaymentIdQuery {PaymentId = paymentId}, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Returns a payment record by order ID.
    /// </summary>
    /// <response code="200">Returns payment record</response>
    /// <response code="403">User is not authorized to access this order</response>
    [HttpGet("get-payment-by-order/{orderId}")]
    [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPaymentByOrderId(int orderId, IAuthorizationService authService,
        CancellationToken cancellationToken)
    {
        var orderPolicy = await authService.AuthorizeAsync(User, orderId, "OrderAccess");
        if (!orderPolicy.Succeeded) return Forbid();
        var response = await mediator.Send(new GetPaymentByOrderIdQuery {OrderId = orderId}, cancellationToken);
        return Ok(response);
    }
}