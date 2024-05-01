namespace Stock.PublisherSystem.SharedKernel;

public abstract class BaseEntity<TId>
{
    public TId? Id { get; protected set; }
    public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
}
