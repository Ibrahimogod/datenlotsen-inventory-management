using System.Net;

namespace Datenlotsen.InventoryManagement.API.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
    }
}