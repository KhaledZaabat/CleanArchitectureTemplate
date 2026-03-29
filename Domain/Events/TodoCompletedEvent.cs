namespace CWM.CleanArchitecture.Domain.Events;

public record TodoCompletedEvent(Guid Id, string Title, DateTimeOffset CompletedAt);
