using ContactAgenda.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace ContactAgenda.Api.Middleware;

/// <summary>
/// Global exception handler middleware
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "Ocorreu um erro interno no servidor";
        var errors = new Dictionary<string, string[]>();

        switch (exception)
        {
            case FluentValidation.ValidationException validationEx:
                statusCode = HttpStatusCode.BadRequest;
                message = "Falha na validação";
                errors = validationEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray());
                break;

            case ContactNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                break;

            case DuplicateEmailException:
                statusCode = HttpStatusCode.Conflict;
                message = exception.Message;
                break;

            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                message = exception.Message;
                break;

            case ArgumentException argEx:
                statusCode = HttpStatusCode.BadRequest;
                message = "Falha na validação";
                errors.Add(argEx.ParamName ?? "field", new[] { argEx.Message });
                break;

            case DomainException domainEx:
                statusCode = HttpStatusCode.BadRequest;
                message = domainEx.Message;
                break;
        }

        var response = new ErrorResponse
        {
            StatusCode = (int)statusCode,
            Message = message,
            Errors = errors.Any() ? errors : null
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}

/// <summary>
/// Error response model
/// </summary>
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, string[]>? Errors { get; set; }
}
