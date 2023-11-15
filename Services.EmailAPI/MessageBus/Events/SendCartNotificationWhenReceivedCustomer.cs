using MasstTransitRabbitMQ.Contract.Abstraction;
using MasstTransitRabbitMQ.Contract.IntergrationEvents;
using MediatR;

namespace Services.EmailAPI.MessageBus.Events
{
    public class SendCartNotificationWhenReceivedCustomer : Consumer<DomainEvent.RegisterNotification>
    {
        public SendCartNotificationWhenReceivedCustomer(ISender sender) : base(sender)
        {
        }
    }
}
