using MassTransit;
using Services.AuthAPI.Configuration;

namespace Services.AuthAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMassTransitRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var MasstransitConfiguration = new MasstransitConfiguration();
            configuration.GetSection(nameof(MasstransitConfiguration)).Bind(MasstransitConfiguration);

            services.AddMassTransit(mt =>
            {
                mt.UsingRabbitMq((context, bus) =>
                {
                    bus.Host(MasstransitConfiguration.Host, MasstransitConfiguration.VHost, h =>
                    {
                        h.Username(MasstransitConfiguration.UserName);
                        h.Password(MasstransitConfiguration.UserName);
                    });
                });
            });

            return services;
        }
    }
}
