namespace CWM.CleanArchitecture.Api.Controllers;

using CWM.CleanArchitecture.Application.Features.ToDo.GetTodoById;
using global::CWM.CleanArchitecture.Application.Features.ToDo.CompleteTodo;
using global::CWM.CleanArchitecture.Application.Features.ToDo.CreateToDo;
using global::CWM.CleanArchitecture.Application.Features.ToDo.DeleteTodo;
using global::CWM.CleanArchitecture.Application.Features.ToDo.GetTodos;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

[ApiController]
[Route("api/todos")]
public sealed class TodosController(IMessageBus _bus) : ControllerBase
{

    // GET api/todos?isCompleted=false
    [HttpGet]
    [ProducesResponseType<List<TodoDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool? isCompleted,
        CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<List<TodoDto>>(
            new GetTodosQuery(isCompleted), ct);

        return Ok(result);
    }

    // GET api/todos/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType<TodoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<TodoDto>(
            new GetTodoByIdQuery(id), ct);

        return Ok(result);
    }

    // POST api/todos
    [HttpPost]
    [ProducesResponseType<Guid>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTodoCommand cmd,
        CancellationToken ct)
    {
        var id = await _bus.InvokeAsync<Guid>(cmd, ct);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    // PUT api/todos/{id}/complete
    [HttpPut("{id:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Complete(Guid id, CancellationToken ct)
    {
        await _bus.InvokeAsync(new CompleteTodoCommand(id), ct);
        return NoContent();
    }

    // DELETE api/todos/{id}
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _bus.InvokeAsync(new DeleteTodoCommand(id), ct);
        return NoContent();
    }
}
