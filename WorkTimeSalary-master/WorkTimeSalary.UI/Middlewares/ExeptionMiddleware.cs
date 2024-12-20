using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WorkTimeSalary.UI.Middlewares
{
    public class ExeptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExeptionMiddleware> _logger;

        public ExeptionMiddleware(RequestDelegate next, ILogger<ExeptionMiddleware> logger)
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
            catch (System.Exception ex)
            {
                _logger.LogError($"An error occurred: {ex}");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, System.Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            var response = new
            {
                title = GetTitle(exception),
                status = statusCode,
                detail = exception.Message,
                errors = GetErrors(exception)
            };
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            var jsonResponse = JsonConvert.SerializeObject(response);

            await httpContext.Response.WriteAsync(jsonResponse);
        }
        private static int GetStatusCode(System.Exception exception) =>
            exception switch
            {
                BadHttpRequestException => StatusCodes.Status400BadRequest,
                DirectoryNotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError
            };
        private static string GetTitle(System.Exception exception)
        {
            return exception switch
            {
                ApplicationException applicationException => applicationException.Message,
                _ => "Server Error"
            };
        }

        private static IReadOnlyDictionary<string, string[]> GetErrors(System.Exception exception)
        {
            IReadOnlyDictionary<string, string[]> errors = null;
            if (exception is ValidationException validationException)
            {
                errors = (IReadOnlyDictionary<string, string[]>?)validationException?.Data;
            }
            return errors;
        }
    }
}
