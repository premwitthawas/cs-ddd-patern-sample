namespace Shared.Messaging.Event;

public record IntegrateEvent {
    public Guid EventID => Guid.NewGuid();
    public DateTime OccuredOn => DateTime.Now;
    public string EventType => GetType().AssemblyQualifiedName!;
};