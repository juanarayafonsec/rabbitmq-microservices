using MicroRabbit.Shared.Domain.Events;

namespace MicroRabbit.Shared.Domain.Bus;
public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
{
    Task Handle(TEvent @event);
}

public interface IEventHandler
{

}
