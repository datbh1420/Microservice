using MassTransit;
using MediatR;

namespace MasstTransitRabbitMQ.Contract.Abstraction.IMessage
{
    [ExcludeFromTopology]
    public interface IMessage : IRequest
    {
        public Guid Id { get; set; }
        public DateTimeOffset TimeSpan { get; set; }
    }
}
