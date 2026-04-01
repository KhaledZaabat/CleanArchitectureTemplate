using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Features.ToDo.DeleteTodo;

public record DeleteTodoCommand(Guid Id);
