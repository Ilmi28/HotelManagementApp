using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.AppProblemDetails;

public class ConflictProblemDetails : ProblemDetails
{
    public ConflictProblemDetails(string? detail)
    {
        Title = "Conflict";
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10";
        Status = StatusCodes.Status409Conflict;
        Detail = detail;
    }
}
