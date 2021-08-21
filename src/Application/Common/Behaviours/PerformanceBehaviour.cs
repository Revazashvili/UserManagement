using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest,TResponse> : IPipelineBehavior<TRequest,TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IConfiguration _configuration;
        private readonly Stopwatch _timer;

        public PerformanceBehaviour(ILogger<TRequest> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _timer = new();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();
            
            var response = await next();
            
            _timer.Stop();
            
            var longRunningRequestTime = Convert.ToInt32(_configuration.GetSection("LongRunningRequestTime").Value);
            var elapsedMilliseconds = _timer.ElapsedMilliseconds;
            if (elapsedMilliseconds > longRunningRequestTime)
            {
                _logger.LogWarning("Long Running Request {Name} ({ElapsedMilliseconds} milliseconds) {@request}",
                    typeof(TRequest).Name,elapsedMilliseconds,request);
            }
            
            return response;
        }
    }
}