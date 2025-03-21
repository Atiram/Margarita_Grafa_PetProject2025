using System.Diagnostics;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQConsumer.Utilities;

public class RabbitMqListener : BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private string hostName;
    private string queueName;

    public RabbitMqListener(IConfiguration configuration)
    {
        this.hostName = configuration.GetSection("RabbitMqSettings:HostName").Value ?? throw new ArgumentException(NotificationMessages.HostSectionMissingErrorMessage);
        this.queueName = configuration.GetSection("RabbitMqSettings:QueueName").Value ?? throw new ArgumentException(NotificationMessages.QueueNameSectionMissingErrorMessage);

        var factory = new ConnectionFactory { HostName = this.hostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: this.queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());

            Debug.WriteLine(string.Format(NotificationMessages.InformationMessage, content));

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queueName, false, consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}