using CWM.CleanArchitecture.Application.Interfaces;
using CWM.CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CWM.CleanArchitecture.Application.Features.ToDo.GetTodos;

public class GetTodosHandler(IRepository<TodoItem> _repo)
{
    public async Task<List<TodoDto>> Handle(GetTodosQuery query, CancellationToken ct)
    {
        var q = _repo.Query();

        if (query.IsCompleted.HasValue)
            q = q.Where(t => t.IsCompleted == query.IsCompleted.Value);

        return await q
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TodoDto(
                t.Id,
                t.Title,
                t.Description,
                t.IsCompleted,
                t.CompletedAt,
                t.CreatedAt))
            .ToListAsync(ct);
    }
}
