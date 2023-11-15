using Mango.Services.EmailAPI.Services;
using MasstTransitRabbitMQ.Contract.IntergrationEvents;
using MediatR;
using Services.EmailAPI.Models.DTO;

namespace Services.EmailAPI.UseCases.Events
{
    public class SendCartNotificationEmailEventConsumerHandler : IRequestHandler<DomainEvent.EmailCartNotification>
    {
        private readonly IEmailService emailService;

        public SendCartNotificationEmailEventConsumerHandler(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public async Task Handle(DomainEvent.EmailCartNotification request, CancellationToken cancellationToken)
        {
            await emailService.EmailCartAndLog(request.Content as CartDTO);
        }
    }
}
