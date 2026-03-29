using System;
using System.Collections.Generic;
using System.Text;

namespace CWM.CleanArchitecture.Application.Features.ToDo.CreateToDo;

public record CreateTodoCommand(string Title, string? Description);
