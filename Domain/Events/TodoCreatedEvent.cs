using System;
using System.Collections.Generic;
using System.Text;

namespace CWM.CleanArchitecture.Domain.Events;

public record TodoCreatedEvent(Guid Id, string Title);
