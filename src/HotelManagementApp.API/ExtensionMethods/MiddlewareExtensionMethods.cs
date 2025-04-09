﻿using HotelManagementApp.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementApp.API.ExtensionMethods
{
    public static class MiddlewareExtensionMethods
    {
        public static IApplicationBuilder UseAppExceptionHandler(this IApplicationBuilder app)
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
                            UserExistsException => StatusCodes.Status409Conflict,
                            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                            _ => StatusCodes.Status500InternalServerError
                        },
                        Detail = exception?.Message,
                        Instance = context.Request.Path
                    };
                    context.Response.StatusCode = problemDetails.Status.Value;
                    await context.Response.WriteAsJsonAsync(problemDetails);
                });
            });
            return app;
        }
    }
}
