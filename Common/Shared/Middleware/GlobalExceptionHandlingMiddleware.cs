using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using System.Text.Json;

namespace Shared.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, "Global exception handler has caught an exception!");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception) 
        {
            context.Response.ContentType = "application/json";

            var statusCode = (int)StatusCodes.Status500InternalServerError;

            var title = "Internal Server Error";

            switch (exception)
            {
                case DomainException:
                    {
                        statusCode = (int)StatusCodes.Status400BadRequest;
                        title = "Bad Requst";
                        break;
                    }
                case NotFoundException:
                    {
                        statusCode = (int)StatusCodes.Status404NotFound;
                        title = "Not Found";
                        break;
                    }
            }

            context.Response.StatusCode = statusCode;

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            var json = JsonSerializer.Serialize(problem);

            await context.Response.WriteAsync(json);
        }
    }
}
