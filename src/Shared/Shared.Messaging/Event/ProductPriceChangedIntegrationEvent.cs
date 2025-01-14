namespace Shared.Messaging.Event;

public record ProductPriceChangedIntegrationEvent : IntegrateEvent
{
    public Guid ProductId {get; set;} = default;
    public string Name { get; set; } = default!;
    public List<string> Category { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string ImageFile { get; set; } = default!;    
    public decimal Price { get; set; } = default!;  
}
