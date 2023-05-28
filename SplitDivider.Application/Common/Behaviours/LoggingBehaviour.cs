using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using SplitDivider.Application.Common.Interfaces;

namespace SplitDivider.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;

    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task Process(TRequest request, CancellationToken _)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("SplitDivider Request: {Name} {@Request}",
            requestName, request);
    }
}
