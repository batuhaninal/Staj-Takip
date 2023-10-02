using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StajTakip.Business.Abstract;
using StajTakip.Business.Concrete;
using StajTakip.DataAccess.Abstract;
using StajTakip.DataAccess.Concrete.EntityFramework.Repositories;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StajTakip.Business.BackgroundServices
{
    public class MessageSenderBackgroundService : BackgroundService
    {
        private readonly RabbitMQClientService _rabbitmqClientService;
        private readonly IMessageRepository _repository;
        private IModel _channel;

        public MessageSenderBackgroundService(RabbitMQClientService rabbitmqClientService, IMessageRepository repository)
        {
            _rabbitmqClientService = rabbitmqClientService;
            _repository = repository;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitmqClientService.Connect();

            //_channel.QueueBind("queue-message", RabbitMQClientService.ExchangeName, "message-route");

            _channel.QueueBind(RabbitMQClientService.QueueName, RabbitMQClientService.ExchangeName, "message-route");

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
            var createdMessage = JsonSerializer.Deserialize<CreatedMessageMessage>(Encoding.UTF8.GetString(@event.Body.ToArray()));

            _repository.Add(new Message
            {
                SenderUserId = createdMessage.SenderUserId,
                MessageDetail = createdMessage.MessageDetail,
                Subject = createdMessage.Subject,
                ReceiverUserId = createdMessage.ReceiverUserId,
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
