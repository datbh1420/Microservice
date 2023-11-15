using MassTransit;
using MasstTransitRabbitMQ.Contract.Abstraction.IMessage;
using MediatR;

namespace MasstTransitRabbitMQ.Contract.Abstraction
{
    public abstract class Consumer<TMessage> : IConsumer<TMessage>
        where TMessage : class, INotificationEvent
    {
        private readonly ISender sender;

        public Consumer(ISender sender)
        {
            this.sender = sender;
        }
        public async Task Consume(ConsumeContext<TMessage> context)
            => await sender.Send(context.Message);
    }
}
