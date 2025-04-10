using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.AppProblemDetails;

public class UnauthorizedProblemDetail : ProblemDetails
{
    public UnauthorizedProblemDetail(string? detail)
    {
        Title = "Unauthorized";
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.2";
        Status = StatusCodes.Status401Unauthorized;
        Detail = detail;
    }
}
