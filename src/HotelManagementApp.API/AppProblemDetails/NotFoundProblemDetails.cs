using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.AppProblemDetails;

public class NotFoundProblemDetails : ProblemDetails
{
    public NotFoundProblemDetails(string? detail)
    {
        Title = "Not Found";
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1";
        Status = StatusCodes.Status404NotFound;
        Detail = detail;
    }
}
