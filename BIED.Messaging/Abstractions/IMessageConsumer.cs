using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BIED.Messaging.Abstractions
{
    interface IMessageConsumer<TMessage>
    {
        Task ProcessMessageAsync(TMessage message);
    }
}
