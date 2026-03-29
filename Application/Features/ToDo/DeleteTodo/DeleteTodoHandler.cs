using CWM.CleanArchitecture.Application.Interfaces;
using CWM.CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace CWM.CleanArchitecture.Application.Features.ToDo.DeleteTodo;

public class DeleteTodoHandler(IRepository<TodoItem> _repo)
{

    public async Task Handle(DeleteTodoCommand cmd, CancellationToken ct)
    {
        var todo = await _repo.QueryTracked()
            .FirstOrDefaultAsync(t => t.Id == cmd.Id, ct)
            ?? throw new InvalidOperationException($"TodoItem with id '{cmd.Id}' was not found.");

        _repo.Delete(todo);
    }
}
