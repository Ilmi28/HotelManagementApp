using HotelManagementApp.Core.Exceptions.BaseExceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API
{
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

                    var problemDetails = new ProblemDetails
                    {
                        Status = exception switch
                        {
                            ConflictException => StatusCodes.Status409Conflict,
                            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        },
                        Detail = exception?.Message,
                        Instance = context.Request.Path
                    };
                    context.Response.StatusCode = problemDetails.Status.Value;
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
}
