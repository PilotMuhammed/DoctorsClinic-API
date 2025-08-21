namespace DoctorClinic.Api.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"Received a {context.Request.Method} request to {context.Request.Path}");

            await _next(context);


            var statusCode = context.Response.StatusCode;
            if (statusCode >= 200 && statusCode < 300)
            {
                _logger.LogInformation($"Response status code: {statusCode}");
            }
            else if (statusCode >= 400 && statusCode < 500)
            {
                _logger.LogWarning($"Client error with status code: {statusCode}");
            }
            else if (statusCode >= 500)
            {
                _logger.LogError($"Server error with status code: {statusCode}");
            }
            else
            {
                _logger.LogInformation($"Unexpected status code: {statusCode}");
            }
        }
    }
}
