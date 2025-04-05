using HotelManagementApp.Core.Exceptions;

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
                    await context.Response.WriteAsync(ex.Message);
                    return;
                case ArgumentNullException:
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid input.");
                    return;
                case UserAlreadyExistsException:
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync(ex.Message);
                    return;
                case VIPException:
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync(ex.Message);
                    return;
                case BlacklistException:
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync(ex.Message);
                    return;
                case Exception:
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync(ex.Message);
                    return;
            }
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An error occurred. Please try again later.");
        }
    }
}
