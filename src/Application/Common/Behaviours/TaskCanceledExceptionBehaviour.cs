using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;

public class TaskCanceledExceptionBehaviour<TRequest,TResponse> : IPipelineBehavior<TRequest,TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public TaskCanceledExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (TaskCanceledException taskCanceledException)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(taskCanceledException, "Request: Task Canceled Exception for Request {Name} {@Request}", 
                requestName, Response.Fail<string>($"Task Canceled Exception: {request}"));
            throw;
        }
    }
}