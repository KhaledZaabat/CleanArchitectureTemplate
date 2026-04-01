using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Events;
using Wolverine;
using Wolverine.Attributes;

namespace CleanArchitecture.Application.Features.ToDo.CreateToDo;

public class CreateTodoHandler(IRepository<TodoItem> _repo)
{

    public async Task<(Guid, OutgoingMessages)> Handle(
        CreateTodoCommand cmd,
        CancellationToken ct)
    {
        var todo = new TodoItem
        {
            Title = cmd.Title,
            Description = cmd.Description
        };

        await _repo.AddAsync(todo, ct);




        OutgoingMessages events = new OutgoingMessages { new TodoCreatedEvent(todo.Id, todo.Title) };
           
   

        return (todo.Id, events);
    }
}
