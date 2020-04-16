using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BIED.Messaging.Abstractions
{
    public interface IMessageProducer
    {
        Task SendAsync<T>(T message, string exchangeName, string routeKey = "", string exchangeType = "topic");
    }
}
