using MediatR;
using Pros.Application.Results;
using Pros.Controllers;
using System.Reflection;
using System.Text.Json;

namespace Pros.Application.Behaviours
{
    public class LoggingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest
    {
        private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

        public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {

            var requestName = typeof(TRequest).Name;
            var requestGuid = Guid.NewGuid().ToString();

            var requestNameWithGuid = $"{requestName} [{requestGuid}]";

            _logger.LogInformation("Starting request {@RequstName}, {@DateTimeUtc}",
                //typeof(TRequest).Name,
                requestNameWithGuid,
                DateTime.UtcNow);

            var result  = await next();


            _logger.LogInformation("Completed request {@RequstName}, {@DateTimeUtc}",
                //typeof(TRequest).Name,
                requestNameWithGuid,
                DateTime.UtcNow);

            return result;
        }
    }
}
