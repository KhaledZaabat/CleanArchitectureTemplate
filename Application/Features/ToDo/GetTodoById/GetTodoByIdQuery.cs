using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Application.Features.ToDo.GetTodos;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.ToDo.GetTodoById;

public record GetTodoByIdQuery(Guid Id);



public class GetTodoByIdHandler
{
    private readonly IRepository<TodoItem> _repo;

    public GetTodoByIdHandler(IRepository<TodoItem> repo) => _repo = repo;

    public async Task<TodoDto> Handle(GetTodoByIdQuery query, CancellationToken ct)
    {
        return await _repo.Query()
            .Where(t => t.Id == query.Id)
            .Select(t => new TodoDto(
                t.Id,
                t.Title,
                t.Description,
                t.IsCompleted,
                t.CompletedAt,
                t.CreatedAt))
            .FirstOrDefaultAsync(ct)
            ?? throw new InvalidOperationException($"TodoItem with id '{query.Id}' was not found.");
    }
}
