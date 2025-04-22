using System.Text;
using System.Text.Json;
using Clinic.Domain;
using ClinicService.BLL.RabbitMqProducer;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

public class RabbitMqService : IRabbitMqService
{
    private string hostName;
    private string queueName;
    private const string RabbitMqSettingsHostName = "RabbitMqSettings:HostName";
    private const string RabbitMqSettingsQueueName = "RabbitMqSettings:QueueName";

    public RabbitMqService(IConfiguration configuration)
    {
        this.hostName = configuration.GetSection(RabbitMqSettingsHostName).Value ??
            throw new ArgumentException(string.Format(NotificationMessages.SectionMissingErrorMessage, RabbitMqSettingsHostName));
        this.queueName = configuration.GetSection(RabbitMqSettingsQueueName).Value ??
            throw new ArgumentException(string.Format(NotificationMessages.SectionMissingErrorMessage, RabbitMqSettingsQueueName));
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