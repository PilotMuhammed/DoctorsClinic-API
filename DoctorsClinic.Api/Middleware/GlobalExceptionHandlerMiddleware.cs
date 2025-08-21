using DoctorsClinic.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DoctorsClinic.Api.Middlewares
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
                ArgumentNullException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };

            var problemDetails = new ResponseDto<ProblemDetails>
            {
                Error = true,
                Message = "An error occurred",
                Data = new ProblemDetails
                {
                    Title = exception.Message,
                    Status = (int)statusCode,
                    Detail = exception.InnerException?.Message,
                    Instance = context.Request.Path,
                }
            };

            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}