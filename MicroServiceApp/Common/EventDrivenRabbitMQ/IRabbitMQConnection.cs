using System;
using RabbitMQ.Client;

namespace EventDrivenRabbitMQ
{
    // IDisposable deletes the connection after its being constructed
    public interface IRabbitMQConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();
    }
}
