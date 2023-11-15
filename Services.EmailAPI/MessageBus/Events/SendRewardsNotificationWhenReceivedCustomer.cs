using MasstTransitRabbitMQ.Contract.Abstraction;
using MasstTransitRabbitMQ.Contract.IntergrationEvents;
using MediatR;

namespace Services.EmailAPI.MessageBus.Events
{
    public class SendRewardsNotificationWhenReceivedCustomer : Consumer<DomainEvent.RewardsNotification>
    {
        public SendRewardsNotificationWhenReceivedCustomer(ISender sender) : base(sender)
        {
        }
    }
}
