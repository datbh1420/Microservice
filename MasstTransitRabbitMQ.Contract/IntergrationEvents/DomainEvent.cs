using MasstTransitRabbitMQ.Contract.Abstraction.IMessage;

namespace MasstTransitRabbitMQ.Contract.IntergrationEvents
{
    public static class DomainEvent
    {
        public record class RegisterNotification : INotificationEvent
        {
            public string Title { get; set; }
            public object Content { get; set; }
            public string Type { get; set; }
            public Guid Id { get; set; }
            public DateTimeOffset TimeSpan { get; set; }
        }

        public record class EmailCartNotification : INotificationEvent
        {
            public string Title { get; set; }
            public object Content { get; set; }
            public string Type { get; set; }
            public Guid Id { get; set; }
            public DateTimeOffset TimeSpan { get; set; }
        }

        public record class RewardsNotification : INotificationEvent
        {
            public string Title { get; set; }
            public object Content { get; set; }
            public string Type { get; set; }
            public Guid Id { get; set; }
            public DateTimeOffset TimeSpan { get; set; }
        }
    }
}
