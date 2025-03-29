using HotelManagementApp.Core.Exceptions.Database;
using HotelManagementApp.Core.Exceptions.UserExceptions;

namespace HotelManagementApp.API.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptions(context, ex);
            }
        }

        public async Task HandleExceptions(HttpContext context, Exception ex)
        {
            switch (ex)
            {
                case UnauthorizedAccessException:
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized access.");
                    return;
                case DatabaseErrorException:
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("Database error occurred.");
                    return;
                case ArgumentNullException:
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid input.");
                    return;
                case UserWithEmailAlreadyExistsException:
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("User with this email already exists.");
                    return;
                case UserWithUserNameAlreadyExistsException:
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("User with this username already exists.");
                    return;
                case UserAlreadyExistsException:
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("User already exists.");
                    return;

            }
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An error occurred. Please try again later.");
        }
    }
}
