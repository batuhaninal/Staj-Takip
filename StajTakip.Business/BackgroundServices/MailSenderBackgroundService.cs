using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StajTakip.Business.Abstract;
using StajTakip.Business.Concrete;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StajTakip.Business.BackgroundServices
{
    public class MailSenderBackgroundService : BackgroundService
    {
        private readonly RabbitMQClientService _rabbitmqClientService;
        private readonly ILogger<MailSenderBackgroundService> _logger;
        private readonly IMailService _mailService;
        private IModel _channel;

        public MailSenderBackgroundService(RabbitMQClientService rabbitmqClientService, ILogger<MailSenderBackgroundService> logger, IMailService mailService)
        {
            _rabbitmqClientService = rabbitmqClientService;
            _logger = logger;
            _mailService = mailService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitmqClientService.Connect();

            // Queue binded :)
            _channel.QueueBind(RabbitMQClientService.QueueName, RabbitMQClientService.ExchangeName, RabbitMQClientService.Routing);

            _channel.BasicQos(0, 1, false);

            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            _channel.BasicConsume(RabbitMQClientService.QueueName, false, consumer);

            consumer.Received += Consumer_Received;

            return Task.CompletedTask;
        }

        private Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            var createdMailMessage = JsonSerializer.Deserialize<CreatedMailMessage>(Encoding.UTF8.GetString(@event.Body.ToArray()));

            _mailService.Send(new Entities.DTOs.EmailSendDto
            {
                Message = createdMailMessage.MessageBody,
                Subject = createdMailMessage.Subject,
                ReceiverMail = createdMailMessage.ReceiverMail,
            });

            _channel.BasicAck(@event.DeliveryTag, false);

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
