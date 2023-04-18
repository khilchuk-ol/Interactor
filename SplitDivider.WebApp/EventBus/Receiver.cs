using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Values.EventBus.Common;
using SplitDivider.Application.Common.Interfaces;

namespace SplitDivider.WebApp.EventBus;

public class Receiver : IEventBusReceiver
{
    private IAsyncConnectionFactory _connectionFactory;

    private ISet<(BaseEvent e, IEventBusEventHandler handler)> _eventSubscriptions = new HashSet<(BaseEvent e, IEventBusEventHandler handler)>();

    public Receiver(IAsyncConnectionFactory connFactory)
    {
        _connectionFactory = connFactory;
    }

    public void SubscribeToEvent(BaseEvent e, IEventBusEventHandler handler)
    {
        _eventSubscriptions.Add((e, handler));
    }

    public void StartReceiving()
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        foreach (var subscription in _eventSubscriptions)
        {
            channel.QueueDeclare(queue: subscription.e.GetEventType(),
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                var eventData = subscription.e.FromEventData(message);
                
                return subscription.handler.Handle(eventData);
            };
            
            channel.BasicConsume(queue: subscription.e.GetEventType(),
                autoAck: true,
                consumer: consumer);
        }
    }
}