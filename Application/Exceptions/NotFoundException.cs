using System;
using System.Collections.Generic;
using System.Text;

namespace CWM.CleanArchitecture.Application.Exceptions;

public sealed class NotFoundException(string entity, object key)
    : Exception($"{entity} with id '{key}' was not found.");
