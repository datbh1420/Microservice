using Mango.Services.EmailAPI.Services;
using MasstTransitRabbitMQ.Contract.IntergrationEvents;
using MediatR;

namespace Services.EmailAPI.UseCases.Events
{
    public class SendRegisterNotificationEmailEventConsumerHandler : IRequestHandler<DomainEvent.RegisterNotification>
    {
        private readonly IEmailService emailService;

        public SendRegisterNotificationEmailEventConsumerHandler(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public async Task Handle(DomainEvent.RegisterNotification request, CancellationToken cancellationToken)
        {
            await emailService.RegisterUserEmailAndLog(request.Content.ToString());
        }
    }
}
