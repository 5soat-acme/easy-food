namespace EF.Domain.Commons.Messages;

public abstract class Message
{
    protected Message()
    {
        MessageType = GetType().Name;
        Timestamp = DateTime.Now;
    }

    public DateTime Timestamp { get; private set; }
    public string MessageType { get; protected set; }
    public Guid AggregateId { get; protected set; }
}