using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Features.ToDo.GetTodos;

public record GetTodosQuery(bool? IsCompleted = null);
