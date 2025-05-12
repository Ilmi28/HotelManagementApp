using HotelManagementApp.Application.CQRS.Review.Add;
using HotelManagementApp.Application.CQRS.Review.GetAll;
using HotelManagementApp.Application.CQRS.Review.GetByGuest;
using HotelManagementApp.Application.CQRS.Review.GetById;
using HotelManagementApp.Application.CQRS.Review.GetByHotel;
using HotelManagementApp.Application.CQRS.Review.Remove;
using HotelManagementApp.Application.CQRS.Review.Update;
using HotelManagementApp.Application.CQRS.Review.UpdateReviewImages;
using HotelManagementApp.Application.Responses.HotelResponses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.Controllers;

[ApiController]
[Route("api/review")]
public class ReviewController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Guest")]
    public async Task<IActionResult> AddReview(AddReviewCommand command, CancellationToken ct)
    {
        await mediator.Send(command, ct);
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<HotelReviewResponse>>> GetAllReviews(CancellationToken ct)
    {
        var reviews = await mediator.Send(new GetAllReviewsQuery(), ct);
        return Ok(reviews);
    }

    [HttpGet("guest/{guestId}")]
    public async Task<ActionResult<ICollection<HotelReviewResponse>>> GetReviewsByGuest(string guestId, CancellationToken ct)
    {
        var reviews = await mediator.Send(new GetReviewsByGuestIdQuery() { GuestId = guestId }, ct);
        return Ok(reviews);
    }

    [HttpGet("hotel/{hotelId}")]
    public async Task<ActionResult<ICollection<HotelReviewResponse>>> GetReviewsByHotel(int hotelId, CancellationToken ct)
    {
        var reviews = await mediator.Send(new GetReviewsByHotelIdQuery() { HotelId = hotelId });
        return Ok(reviews);
    }

    [HttpGet("{reviewId}")]
    public async Task<ActionResult<HotelReviewResponse>> GetReviewById(int reviewId, CancellationToken ct)
    {
        var review = await mediator.Send(new GetReviewByIdQuery { ReviewId = reviewId }, ct);
        return Ok(review);
    }

    [HttpDelete("{reviewId}")]
    [Authorize]
    public async Task<IActionResult> RemoveReview(int reviewId, IAuthorizationService authService, CancellationToken ct)
    {
        var reviewAccess = await authService.AuthorizeAsync(User, reviewId, "ReviewAccess");
        if (!reviewAccess.Succeeded) return Forbid();
        await mediator.Send(new RemoveReviewCommand { ReviewId = reviewId }, ct);
        return NoContent();
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateReview([FromBody] UpdateReviewCommand cmd, IAuthorizationService authService,CancellationToken ct)
    {
        var reviewAccess = await authService.AuthorizeAsync(User, cmd.ReviewId, "ReviewAccess");
        if (!reviewAccess.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    [HttpPut("images")]
    [Authorize]
    public async Task<IActionResult> UpdateReviewImages([FromBody] UpdateReviewImagesCommand cmd, IAuthorizationService authService, CancellationToken ct)
    {
        var reviewAccess = await authService.AuthorizeAsync(User, cmd.ReviewId, "ReviewAccess");
        if (!reviewAccess.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }
}