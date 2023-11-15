using Mango.Services.EmailAPI.Services;
using MasstTransitRabbitMQ.Contract.IntergrationEvents;
using MediatR;
using Services.EmailAPI.Models.DTO;

namespace Services.EmailAPI.UseCases.Events
{
    public class SendRewardsNotificationEmailEventConsumerHandler : IRequestHandler<DomainEvent.RewardsNotification>
    {
        private readonly IEmailService emailService;

        public SendRewardsNotificationEmailEventConsumerHandler(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public async Task Handle(DomainEvent.RewardsNotification request, CancellationToken cancellationToken)
        {
            await emailService.LogOrderPlaced(request.Content as RewardsDTO);
        }
    }
}
