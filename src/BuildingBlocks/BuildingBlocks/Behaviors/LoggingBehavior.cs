﻿using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) :
    IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handling Request {RequestName} - Response {ResponseName} - RequestData {RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = Stopwatch.StartNew();

        var response = await next(cancellationToken);

        timer.Stop();
        var timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 3)
        {
            logger.LogWarning("[PERFORMANCE] The request {RequestName} with data {RequestData} took {TimeTaken} seconds to complete",
                typeof(TRequest).Name, request, timeTaken.TotalSeconds);
        }

        logger.LogInformation("[END] Finished handling request {RequestName} with response {Response} - taken {TimeTaken} seconds",
            typeof(TRequest).Name, response, timeTaken.TotalSeconds);
        return response;
    }
}