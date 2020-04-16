using BIED.Messaging.Abstractions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BIED.Messaging
{
    public abstract class MessageConsumer<TMessage> : IMessageConsumer<TMessage>, IHostedService
    {
        private readonly IConnection connection;
        private readonly string exchange;
        private readonly string routingKey;

        public MessageConsumer(IConnection connection, string exchange, string routingKey)
        {
            this.connection = connection;
            this.exchange = exchange;
            this.routingKey = routingKey;
        }

        public abstract Task ProcessMessageAsync(TMessage message);


        public Task StartAsync(CancellationToken cancellationToken)
        {
            var channel = connection.CreateModel();
            var queue = CreateQueue(channel);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) => await OnMessageReceived(model, ea);

            StartConsuming(channel, queue, consumer);

            return Task.CompletedTask;
        }

        private Task OnMessageReceived(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var json = Encoding.UTF8.GetString(body);

            return ProcessMessageAsync(JsonConvert.DeserializeObject<TMessage>(json));
        }

        private static void StartConsuming(IModel channel, QueueDeclareOk queue, EventingBasicConsumer consumer)
        {
            channel.BasicConsume(queue.QueueName, true, consumer);
        }

        private QueueDeclareOk CreateQueue(IModel channel)
        {
            channel.ExchangeDeclare(exchange, type: ExchangeType.Topic);

            var queue = channel.QueueDeclare();

            channel.QueueBind(queue: queue.QueueName,
                              exchange: exchange,
                              routingKey: routingKey);
            return queue;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

    }
}
