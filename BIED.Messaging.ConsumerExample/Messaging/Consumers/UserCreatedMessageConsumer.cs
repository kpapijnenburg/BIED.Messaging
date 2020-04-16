using BIED.Messaging.ConsumerExample.Messaging.Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BIED.Messaging.ConsumerExample.Messaging.Consumers
{
    public class UserCreatedMessageConsumer : MessageConsumer<UserCreatedMessage>
    {
        public UserCreatedMessageConsumer(IConnection connection) : base(connection, "users.created", "")
        {
        }
        
        public override Task ProcessMessageAsync(UserCreatedMessage message)
        {
            string json = JsonConvert.SerializeObject(message, Formatting.Indented);

            Console.WriteLine("Recieved newly created user:\n");
            Console.WriteLine(json);

            return Task.CompletedTask;
        }
    }
}
