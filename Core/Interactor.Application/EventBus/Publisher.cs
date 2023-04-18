using System.Text;
using Interactor.Application.Common.Interfaces;
using RabbitMQ.Client;
using Shared.Values.EventBus.Common;

namespace Interactor.Application.EventBus;

public class Publisher : IEventBusPublisher
{
    private IAsyncConnectionFactory _connectionFactory;
    
    public Publisher(IAsyncConnectionFactory connFactory)
    {
        _connectionFactory = connFactory;
    }
    
    public void Send(BaseEvent eEvent)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: eEvent.GetEventType(),
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        
        var body = Encoding.UTF8.GetBytes(eEvent.ToEventData());
        
        channel.BasicPublish(exchange: string.Empty,
            routingKey: eEvent.GetContentType(),
            basicProperties: null,
            body: body);
        
        channel.Close();
        connection.Close();
    }
}