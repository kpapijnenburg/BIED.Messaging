using BIED.Messaging.Abstractions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BIED.Messaging
{
    public class MessageProducer : IMessageProducer
    {
        private IConnection connection;

        public MessageProducer(IConnection connection)
        {
            this.connection = connection;
        }
        public async Task SendAsync<T>(T message, string exchangeName, string routeKey = "", string exchangeType = "topic")
        {
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchangeName, exchangeType);

            var bytesToSend = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            await Task.Run(() => channel.BasicPublish(exchangeName, routeKey, properties, bytesToSend));
        }
    }
}
