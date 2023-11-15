namespace MasstTransitRabbitMQ.Contract.Abstraction.IMessage
{
    public interface INotificationEvent : IMessage
    {
        public string Title { get; set; }
        public object Content { get; set; }
        public string Type { get; set; }
    }
}
