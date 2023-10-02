using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Concrete
{
    public class RabbitMQClientService
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private readonly ILogger<RabbitMQClientService> _logger;
        public static string ExchangeName = "MailDirectExchange";
        public static string Routing = "mail-route";
        public static string QueueName = "queue-mail";

        public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();
            // Channel is open?
            if (_channel is { IsOpen: true })
                return _channel;

            // Channel created
            _channel = _connection.CreateModel();

            // Exchange declared
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false);

            // Queue declared for mail
            _channel.QueueDeclare(QueueName, true, false, false);

            // for message
            //_channel.QueueDeclare("queue-message", true, false, false);

            //_channel.QueueBind(QueueName, ExchangeName, Routing);

            _logger.LogInformation("RabbitMQ is connected");

            return _channel;
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("RabbitMQ is disconnected and disposed");
        }

    }
}
