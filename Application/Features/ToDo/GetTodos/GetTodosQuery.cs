using System;
using System.Collections.Generic;
using System.Text;

namespace CWM.CleanArchitecture.Application.Features.ToDo.GetTodos;

public record GetTodosQuery(bool? IsCompleted = null);
