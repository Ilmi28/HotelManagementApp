using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.AppProblemDetails;

public class ForbiddenProblemDetail : ProblemDetails
{
    public ForbiddenProblemDetail(string? detail)
    {
        Title = "Forbidden";
        Status = StatusCodes.Status403Forbidden;
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.3";
        Detail = detail;
    }
}
