using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Ordering.API.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.API.Extentions
{
    public static class ApplicationBuilderExtentions
    {

        public static EventBusRabbitMQConsumer Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            // Get one instance of this
            Listener = app.ApplicationServices.GetService<EventBusRabbitMQConsumer>();
            // When starting and stopping the application
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(OnStart);
            life.ApplicationStopping.Register(OnStop);

            return app;
        }

        private static void OnStart()
        {
            Listener.Consume();
        }

        private static void OnStop()
        {
            Listener.Disconnect();
        }
    }
}
