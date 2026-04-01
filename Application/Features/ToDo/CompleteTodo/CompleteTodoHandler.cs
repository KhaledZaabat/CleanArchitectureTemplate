using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Wolverine;
using Wolverine.Attributes;

namespace CleanArchitecture.Application.Features.ToDo.CompleteTodo;

public class CompleteTodoHandler(IRepository<TodoItem> _repo)
{

    public async Task<OutgoingMessages> Handle(
        CompleteTodoCommand cmd,
        CancellationToken ct)
    {
        var todo = await _repo.QueryTracked()
            .FirstOrDefaultAsync(t => t.Id == cmd.Id, ct)
            ?? throw new InvalidOperationException($"TodoItem with id '{cmd.Id}' was not found."); //throw new NotFoundException(nameof(TodoItem), cmd.Id);

        todo.MarkAsCompleted();
        _repo.Update(todo);

        return new OutgoingMessages
        {
            new TodoCompletedEvent(todo.Id, todo.Title, todo.CompletedAt!.Value)
        };
    }
}
