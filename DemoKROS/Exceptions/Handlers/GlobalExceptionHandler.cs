using System.Net;
using System.Text.Json;
using DemoKROS.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace DemoKROS.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

        if (exception is NotFoundException)
        {
            statusCode = HttpStatusCode.NotFound;
        }
        else if (exception is ValidationException)
        {
            statusCode = HttpStatusCode.BadRequest;
        }

        httpContext.Response.StatusCode = (int)statusCode;
        httpContext.Response.ContentType = "application/json";

        var response = new
        {
            error = exception.Message
        };

        await httpContext.Response.WriteAsync(
            JsonSerializer.Serialize(response),
            cancellationToken);

        return true;
    }
}