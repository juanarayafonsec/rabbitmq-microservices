using MicroRabbit.Shared.Domain.Bus;
using MicroRabbit.Shared.Infrastructure.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Shared.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Register RabbitMQ Bus
            //RabbitMQ connections are expensive to create and the class also keep states
            services.AddSingleton<IEventBus, RabbitMQBus>();
            // Register MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyContainer).Assembly));
            // Register other dependencies as needed
            // Example: services.AddTransient<IYourService, YourServiceImplementation>();
        }
    }
}
