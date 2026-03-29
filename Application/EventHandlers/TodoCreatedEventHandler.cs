using System;
using System.Collections.Generic;
using System.Text;
using CWM.CleanArchitecture.Domain.Events;
using Microsoft.Extensions.Logging;

namespace CWM.CleanArchitecture.Application.EventHandlers;

public class TodoCreatedEventHandler
{
    private readonly ILogger<TodoCreatedEventHandler> _logger;

    public TodoCreatedEventHandler(ILogger<TodoCreatedEventHandler> logger)
        => _logger = logger;

    public Task Handle(TodoCreatedEvent evt)
    {
        
        _logger.LogInformation(
            "Todo created — Id: {Id} | Title: {Title}",
            evt.Id,
            evt.Title);

        return Task.CompletedTask;
    }
}
