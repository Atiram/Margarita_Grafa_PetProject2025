using System.ComponentModel.DataAnnotations;
using System.Net;

namespace ClinicService.API.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (ValidationException ex)
        {
            HandleValidationException(context, ex);
        }
    }
    
    private static void HandleValidationException(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.WriteAsJsonAsync(exception.Message);
    }     
}
