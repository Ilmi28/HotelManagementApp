using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.AppProblemDetails;

public class InternalServerErrorProblemDetails : ProblemDetails
{
    public InternalServerErrorProblemDetails(string? detail)
    {
        Title = "Internal Server Error";
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1";
        Status = StatusCodes.Status500InternalServerError;
        Detail = detail;
    }
}
