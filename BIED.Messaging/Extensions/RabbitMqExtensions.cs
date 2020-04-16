using BIED.Messaging.Abstractions;
using BIED.Messaging.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Threading;

namespace BIED.Messaging.Extensions
{
    public static class RabbitMqExtensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services)
        {
            var config = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<RabbitMqConfig>>();

            var factory = new ConnectionFactory()
            {
                HostName = config.Value.HostName,
                Port = config.Value.Port,
                UserName = config.Value.UserName,
                Password = config.Value.Password
            };

            IConnection connection = null;

            for (int i = 0; i < config.Value.ConnectionRetries; i++)
            {
                try
                {
                    connection = factory.CreateConnection();
                }
                catch (BrokerUnreachableException exception)
                {
                    Console.WriteLine($"{exception.Message} on http://{factory.HostName}:{factory.Port}. Retrying in 5 seconds. {config.Value.ConnectionRetries - i} attempts remaining.");

                    Thread.Sleep(5000);
                }
            }

            if (connection == null)
            {
                throw new BrokerUnreachableException(new Exception("Broker could not be reached within the retry limit."));
            }

            services.AddSingleton((_) => connection);

            services.AddSingleton<IMessageProducer, MessageProducer>();

            return services;
        }
    }
}
