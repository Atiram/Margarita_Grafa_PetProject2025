using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQProducer.RabbitMq;
using RabbitMQProducer.Utilities.NotificationMessages;

public class RabbitMqService : IRabbitMqService
{
    private string hostName;
    private string queueName;

    public RabbitMqService(IConfiguration configuration)
    {
        this.hostName = configuration.GetSection("RabbitMqSettings:HostName").Value ?? throw new ArgumentException(NotificationMessages.HostSectionMissingErrorMessage);
        this.queueName = configuration.GetSection("RabbitMqSettings:QueueName").Value ?? throw new ArgumentException(NotificationMessages.QueueNameSectionMissingErrorMessage);
    }
    public void SendMessage(object obj)
    {
        var message = JsonSerializer.Serialize(obj);
        SendMessage(message);
    }

    public void SendMessage(string message)
    {
        var factory = new ConnectionFactory() { HostName = hostName };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: queueName,
                           durable: false,
                           exclusive: false,
                           autoDelete: false,
                           arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                           routingKey: queueName,
                           basicProperties: null,
                           body: body);
        }
    }
}