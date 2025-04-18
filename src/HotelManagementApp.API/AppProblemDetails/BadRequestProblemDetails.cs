using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.AppProblemDetails;

public class BadRequestProblemDetails : ProblemDetails
{
    public BadRequestProblemDetails(string? detail)
    {
        Title = "Bad Request";
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1";
        Status = StatusCodes.Status400BadRequest;
        Detail = detail;
    }
}
