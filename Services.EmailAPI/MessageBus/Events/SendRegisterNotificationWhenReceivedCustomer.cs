using MasstTransitRabbitMQ.Contract.Abstraction;
using MasstTransitRabbitMQ.Contract.IntergrationEvents;
using MediatR;

namespace Services.EmailAPI.MessageBus.Events
{
    public class SendRegisterNotificationWhenReceivedCustomer : Consumer<DomainEvent.RegisterNotification>
    {
        public SendRegisterNotificationWhenReceivedCustomer(ISender sender) : base(sender)
        {
        }
    }
}
