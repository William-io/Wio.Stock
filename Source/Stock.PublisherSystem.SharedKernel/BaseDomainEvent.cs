using MediatR;

namespace Stock.PublisherSystem.SharedKernel;

public abstract class BaseDomainEvent : INotification
{
    public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
}