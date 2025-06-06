using MicroRabbit.Shared.Domain.Events;

namespace MicroRabbit.Shared.Domain.Commands;

public abstract class Command : Message
{
    public DateTime Timestamp { get; protected set; }
    protected Command()
    {
        Timestamp = DateTime.UtcNow;
    }
}