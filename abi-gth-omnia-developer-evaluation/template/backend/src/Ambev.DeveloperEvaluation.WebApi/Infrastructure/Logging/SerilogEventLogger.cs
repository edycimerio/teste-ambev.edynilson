using Ambev.DeveloperEvaluation.Application.Common.Events;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi.Infrastructure.Logging;

/// <summary>
/// Implementação do IEventLogger usando Serilog.
/// </summary>
public class SerilogEventLogger : IEventLogger
{
    private readonly Serilog.ILogger _logger;

    public SerilogEventLogger(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    public Task LogEventAsync(string eventType, object eventData)
    {
        _logger.Information(
            "Business Event: {EventType} - {@EventData}",
            eventType,
            eventData);

        return Task.CompletedTask;
    }
}
