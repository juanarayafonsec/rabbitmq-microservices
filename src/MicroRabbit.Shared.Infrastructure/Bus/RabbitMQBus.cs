using MediatR;
using MicroRabbit.Shared.Domain.Bus;
using MicroRabbit.Shared.Domain.Commands;
using MicroRabbit.Shared.Domain.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Threading.Tasks;

namespace MicroRabbit.Shared.Infrastructure.Bus;

public sealed class RabbitMQBus(IMediator mediator) : IEventBus
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly Dictionary<string, List<Type>> _handlers = new Dictionary<string, List<Type>>();
    private readonly List<Type> _eventTypes = new List<Type>();

    public Task SendCommand<T>(T command) where T : Command
    {
        return _mediator.Send(command);
    }

    public async Task PublishAsync<T>(T @event) where T : Message
    {
        var eventName = @event.GetType().Name;

        var factory = CreateConnectionFactory();

        await using var conn = await factory.CreateConnectionAsync();
        await using var channel = await conn.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: eventName, durable: false, exclusive: false, autoDelete: false);

        var message = JsonSerializer.Serialize(@event);
        byte[] messageBody = System.Text.Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(
            exchange: "",//defult exchange
            routingKey: eventName,//matches the queue name
            mandatory: true,
            basicProperties: new BasicProperties(),
            body: messageBody
        );

    }

    public async Task Subscribe<T, TH>()
        where T : Event
        where TH : IEventHandler<T>
    {
        var eventName = typeof(T).Name;
        var handlerType = typeof(TH);

        if (!_eventTypes.Contains(typeof(T)))
        {
            _eventTypes.Add(typeof(T));
        }

        if (!_handlers.ContainsKey(eventName))
        {
            _handlers.Add(eventName, new List<Type>());
        }

        if (_handlers[eventName].Contains(handlerType))
        {
            throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
        }

        _handlers[eventName].Add(handlerType);

        await StarBasicConsume<T>();

    }

    private async Task StarBasicConsume<T>() where T : Event
    {
        var factory = CreateConnectionFactory();
        var eventName = typeof(T).Name;

        var conn = await factory.CreateConnectionAsync();
        var channel = await conn.CreateChannelAsync();
        await channel.QueueDeclareAsync(queue: eventName, durable: false, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = System.Text.Encoding.UTF8.GetString(body);
            await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        await channel.BasicConsumeAsync(queue: eventName, autoAck: true, consumer: consumer);

    }

    private ConnectionFactory CreateConnectionFactory()
    {
        return new ConnectionFactory
        {
            UserName = "user",
            Password = "password",
            HostName = "localhost"
        };
    }

}

