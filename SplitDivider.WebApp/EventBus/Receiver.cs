using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Values.EventBus.Common;
using SplitDivider.Application.Common.Interfaces;

namespace SplitDivider.WebApp.EventBus;

public class Receiver : IEventBusReceiver, IDisposable
{
    private IAsyncConnectionFactory _connectionFactory;

    private IConnection _connection;

    private IModel _channel;

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
        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        
        _channel.ExchangeDeclare(
            exchange: "interactor_direct_msg",
            type: "direct",
            durable: true);

        foreach (var subscription in _eventSubscriptions)
        {
            var eventType = subscription.e.GetEventType();
            
            _channel.QueueDeclare(
                queue: eventType,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            
            _channel.QueueBind(
                queue: eventType,
                exchange: "interactor_direct_msg",
                routingKey: eventType);
            
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                var eventData = subscription.e.FromEventData(message);
                
                await subscription.handler.Handle(eventData).ConfigureAwait(true);
            };
            
            _channel.BasicConsume(
                queue: eventType,
                autoAck: true,
                consumer: consumer);
        }
    }

    public void Dispose()
    {
        _connection.Dispose();
        _channel.Dispose();
    }
}