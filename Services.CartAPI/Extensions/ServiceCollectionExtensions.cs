using MassTransit;
using Services.CartAPI.Configuration;

namespace Services.CartAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMassTransitRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var MassTransitConfig = new MasstransitConfiguration();
            configuration.GetSection(nameof(MasstransitConfiguration)).Bind(MassTransitConfig);

            services.AddMassTransit(mt =>
            {
                mt.UsingRabbitMq((context, bus) =>
                {
                    bus.Host(MassTransitConfig.Host, MassTransitConfig.VHost, h =>
                    {
                        h.Username(MassTransitConfig.UserName);
                        h.Password(MassTransitConfig.Password);
                    });
                });
            });
            return services;
        }
    }
}
