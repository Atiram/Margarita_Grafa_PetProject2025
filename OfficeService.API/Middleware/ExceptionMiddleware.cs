using System.ComponentModel.DataAnnotations;
using System.Net;
using Clinic.Domain.Exceptions;

namespace OfficeService.API.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            HandleException(context, ex);
        }
    }

    private static void HandleException(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            ValidationException => (int)HttpStatusCode.BadRequest,
            NotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError,
        };
        context.Response.WriteAsJsonAsync(exception.Message);
    }
}
