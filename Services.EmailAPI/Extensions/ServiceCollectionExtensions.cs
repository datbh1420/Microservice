using MassTransit;
using Services.EmailAPI.Configurations;
using System.Reflection;

namespace Services.EmailAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatR(this IServiceCollection services)
            => services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        public static IServiceCollection AddMassTransitRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var MasstransitConfiguration = new MasstransitConfiguration();
            configuration.GetSection(nameof(MasstransitConfiguration)).Bind(MasstransitConfiguration);

            services.AddMassTransit(mt =>
            {
                mt.AddConsumers(Assembly.GetExecutingAssembly());

                mt.UsingRabbitMq((context, bus) =>
                {
                    bus.Host(MasstransitConfiguration.Host, MasstransitConfiguration.VHost, h =>
                    {
                        h.Username(MasstransitConfiguration.UserName);
                        h.Password(MasstransitConfiguration.UserName);
                    });
                    bus.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
