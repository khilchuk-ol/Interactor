﻿using MediatR;
using Microsoft.Extensions.Logging;

namespace SplitDivider.Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken _)
    {
        try
        {
            return await next().ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogError(ex, "SplitDivider Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw;
        }
    }
}
