using HotelManagementApp.API.AppProblemDetails;
using HotelManagementApp.Core.Exceptions.BaseExceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API;

public static class MiddlewareConfig
{
    public static IApplicationBuilder UseAppMiddleware(this WebApplication app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;


                ProblemDetails problemDetails = exception switch
                {
                    ConflictException => new ConflictProblemDetails(exception.Message),
                    NotFoundException => new NotFoundProblemDetails(exception.Message),
                    UnauthorizedAccessException => new UnauthorizedProblemDetail(exception.Message),
                    ForbiddenException => new ForbiddenProblemDetail(exception.Message),
                    BadRequestException => new BadRequestProblemDetails(exception.Message),
                    _ => new InternalServerErrorProblemDetails(exception!.Message)
                };
                context.Response.StatusCode = problemDetails.Status!.Value;
                await context.Response.WriteAsJsonAsync(problemDetails);
            });
        });

        app.UseStatusCodePages();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}
