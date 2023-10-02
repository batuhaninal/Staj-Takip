using RabbitMQ.Client;
using StajTakip.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace StajTakip.Business.Concrete
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQClientService _rabbitMQClientService;

        public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
        {
            _rabbitMQClientService = rabbitMQClientService;
        }

        public void Publish(CreatedMailMessage createMailMessage)
        {
            var channel = _rabbitMQClientService.Connect();

            var jsonBody = JsonSerializer.Serialize(createMailMessage);

            var byteBody = Encoding.UTF8.GetBytes(jsonBody);

            var properties = channel.CreateBasicProperties();

            properties.Persistent = true;

            channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.Routing, basicProperties: properties, body: byteBody);
        }

        public void Publish(CreatedMessageMessage createdMessageMessage)
        {
            var channel = _rabbitMQClientService.Connect();

            var jsonBody = JsonSerializer.Serialize(createdMessageMessage);

            var byteBody = Encoding.UTF8.GetBytes(jsonBody);

            var properties = channel.CreateBasicProperties();

            properties.Persistent = true;

            channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName, routingKey: "message-route", basicProperties: properties, body: byteBody);
        }
    }
}
