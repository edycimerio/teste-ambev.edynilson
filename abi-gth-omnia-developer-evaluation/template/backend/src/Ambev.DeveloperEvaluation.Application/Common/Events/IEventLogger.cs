namespace Ambev.DeveloperEvaluation.Application.Common.Events;

/// <summary>
/// Interface para logging de eventos de negócio.
/// </summary>
public interface IEventLogger
{
    /// <summary>
    /// Registra um evento de negócio.
    /// </summary>
    /// <param name="eventType">Tipo do evento (ex: SaleCreated, SaleCanceled)</param>
    /// <param name="eventData">Dados do evento em formato de objeto</param>
    Task LogEventAsync(string eventType, object eventData);
}
