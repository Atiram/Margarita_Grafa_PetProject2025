﻿using System.Text;
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
    private const string RabbitMqSettingsHostName = "RabbitMqSettings:HostName";
    private const string RabbitMqSettingsQueueName = "RabbitMqSettings:QueueName";

    public RabbitMqListener(IConfiguration configuration, IServiceScopeFactory scopedfactory)
    {
        this.hostName = configuration.GetSection(RabbitMqSettingsHostName).Value ??
            throw new ArgumentException(string.Format(NotificationMessages.SectionMissingErrorMessage, RabbitMqSettingsHostName));
        this.queueName = configuration.GetSection(RabbitMqSettingsQueueName).Value ??
            throw new ArgumentException(string.Format(NotificationMessages.SectionMissingErrorMessage, RabbitMqSettingsQueueName));

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
                var eventService = scope.ServiceProvider.GetRequiredService<IEventService>();
                CreateEventMail createEventMail = JsonConvert.DeserializeObject<CreateEventMail>(content);
                await eventService.CreateAsync(createEventMail);
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