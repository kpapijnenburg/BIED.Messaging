using System.Threading.Tasks;

namespace BIED.Messaging.Abstractions
{
    interface IMessageConsumer<TMessage>
    {
        Task ProcessMessageAsync(TMessage message);
    }
}
