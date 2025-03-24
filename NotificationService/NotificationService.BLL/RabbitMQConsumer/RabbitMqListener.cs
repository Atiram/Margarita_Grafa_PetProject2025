using System.Text;
using Clinic.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NotificationService.BLL.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqListener : BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private string hostName;
    private string queueName;
    readonly IServiceScopeFactory _scopedfactory;
    public RabbitMqListener(IConfiguration configuration, IServiceScopeFactory scopedfactory)
    {
        this.hostName = configuration.GetSection("RabbitMqSettings:HostName").Value ?? throw new ArgumentException(NotificationMessages.HostSectionMissingErrorMessage);
        this.queueName = configuration.GetSection("RabbitMqSettings:QueueName").Value ?? throw new ArgumentException(NotificationMessages.QueueNameSectionMissingErrorMessage);

        var factory = new ConnectionFactory { HostName = this.hostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: this.queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        _scopedfactory = scopedfactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());

            if (content != null)
            {
                using var scope = _scopedfactory.CreateScope();
                var ser = scope.ServiceProvider.GetRequiredService<IEventService>();
                CreateEventMail createEventMail = JsonConvert.DeserializeObject<CreateEventMail>(content);
                await ser.CreateAsync(createEventMail);
            }
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