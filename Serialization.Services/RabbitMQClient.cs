using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Serialization.Services
{
    public class RabbitMQClient : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMQClient(string hostName, string userName, string password, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = queueName;

            _channel.QueueDeclare(queueName, false, false, false, null);
        }

        public void Send(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish("", _queueName, null, body);
            Console.WriteLine($"Sent: {message}");
        }

        public void Receive()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var receivedMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"Received: {receivedMessage}");
            };

            _channel.BasicConsume(_queueName, true, consumer);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }

}
