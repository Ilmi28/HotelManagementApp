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
[Authorize]
[Route("api/review")]
public class ReviewController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Adds a new review.
    /// </summary>
    /// <response code="204">Review added</response>
    [HttpPost]
    [Authorize(Roles = "Guest")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddReview(AddReviewCommand command, CancellationToken ct)
    {
        await mediator.Send(command, ct);
        return NoContent();
    }

    /// <summary>
    /// Gets all reviews.
    /// </summary>
    /// <response code="200">Returns all reviews</response>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<HotelReviewResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<HotelReviewResponse>>> GetAllReviews(CancellationToken ct)
    {
        var reviews = await mediator.Send(new GetAllReviewsQuery(), ct);
        return Ok(reviews);
    }

    /// <summary>
    /// Gets reviews by guest ID.
    /// </summary>
    /// <response code="200">Returns guest's reviews</response>
    [HttpGet("guest/{guestId}")]
    [ProducesResponseType(typeof(ICollection<HotelReviewResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<HotelReviewResponse>>> GetReviewsByGuest(string guestId, CancellationToken ct)
    {
        var reviews = await mediator.Send(new GetReviewsByGuestIdQuery { GuestId = guestId }, ct);
        return Ok(reviews);
    }

    /// <summary>
    /// Gets reviews by hotel ID.
    /// </summary>
    /// <response code="200">Returns hotel's reviews</response>
    [HttpGet("hotel/{hotelId}")]
    [ProducesResponseType(typeof(ICollection<HotelReviewResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<HotelReviewResponse>>> GetReviewsByHotel(int hotelId, CancellationToken ct)
    {
        var reviews = await mediator.Send(new GetReviewsByHotelIdQuery { HotelId = hotelId }, ct);
        return Ok(reviews);
    }

    /// <summary>
    /// Gets a review by ID.
    /// </summary>
    /// <response code="200">Returns the review</response>
    [HttpGet("{reviewId}")]
    [ProducesResponseType(typeof(ICollection<HotelReviewResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<HotelReviewResponse>> GetReviewById(int reviewId, CancellationToken ct)
    {
        var review = await mediator.Send(new GetReviewByIdQuery { ReviewId = reviewId }, ct);
        return Ok(review);
    }

    /// <summary>
    /// Removes a review.
    /// </summary>
    /// <response code="204">Removed successfully</response>
    /// <response code="403">Access denied</response>
    [HttpDelete("{reviewId}")]
    [Authorize(Roles = "Guest")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemoveReview(int reviewId, IAuthorizationService authService, CancellationToken ct)
    {
        var reviewAccess = await authService.AuthorizeAsync(User, reviewId, "ReviewAccess");
        if (!reviewAccess.Succeeded) return Forbid();
        await mediator.Send(new RemoveReviewCommand { ReviewId = reviewId }, ct);
        return NoContent();
    }

    /// <summary>
    /// Updates a review.
    /// </summary>
    /// <response code="204">Updated successfully</response>
    /// <response code="403">Access denied</response>
    [HttpPut]
    [Authorize(Roles = "Guest")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateReview([FromBody] UpdateReviewCommand cmd, IAuthorizationService authService, CancellationToken ct)
    {
        var reviewAccess = await authService.AuthorizeAsync(User, cmd.ReviewId, "ReviewAccess");
        if (!reviewAccess.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }

    /// <summary>
    /// Updates images of a review.
    /// </summary>
    /// <response code="204">Images updated</response>
    /// <response code="403">Access denied</response>
    [HttpPut("images")]
    [Authorize(Roles = "Guest")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateReviewImages([FromForm] UpdateReviewImagesCommand cmd, IAuthorizationService authService, CancellationToken ct)
    {
        var reviewAccess = await authService.AuthorizeAsync(User, cmd.ReviewId, "ReviewAccess");
        if (!reviewAccess.Succeeded) return Forbid();
        await mediator.Send(cmd, ct);
        return NoContent();
    }
}
