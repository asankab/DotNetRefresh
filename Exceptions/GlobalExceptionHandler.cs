using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Net;

namespace Pros.Exceptions
{
    public class GlobalExceptionHandler: IMiddleware
    {
        private ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch(Exception ex) {
                string message = ex.Message.ToString();

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                _logger.LogError($"Exception Details: {message}");

                var response = new ExceptionDetails()
                {
                    StatusCodes = context.Response.StatusCode,
                    Message = message,
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}
