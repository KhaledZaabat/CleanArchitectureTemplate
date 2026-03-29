using CWM.CleanArchitecture.Domain.Events;
using Microsoft.Extensions.Logging;

namespace CWM.CleanArchitecture.Application.EventHandlers;

public class TodoCompletedEventHandler
{
    private readonly ILogger<TodoCompletedEventHandler> _logger;

    public TodoCompletedEventHandler(ILogger<TodoCompletedEventHandler> logger)
        => _logger = logger;

    public Task Handle(TodoCompletedEvent evt)
    {
        _logger.LogInformation(
            "Todo completed — Id: {Id} | Title: {Title} | At: {CompletedAt}",
            evt.Id,
            evt.Title,
            evt.CompletedAt);

        return Task.CompletedTask;
    }
}
