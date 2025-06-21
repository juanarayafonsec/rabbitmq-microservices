using MicroRabbit.Shared.Domain.Commands;
using MicroRabbit.Shared.Domain.Events;

namespace MicroRabbit.Shared.Domain.Bus;
public interface IEventBus
{
    Task SendCommand<T>(T command) where T : Command;
    Task PublishAsync<T>(T @event) where T : Message;
    Task Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>;
}