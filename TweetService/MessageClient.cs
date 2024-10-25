using EasyNetQ;

namespace TweetService;

public class MessageClient
{
    private readonly IBus _bus;

    public MessageClient(IBus bus)
    {
        _bus = bus;
    }

    public MessageClient()
    {
        _bus = RabbitHutch.CreateBus("host=localhost");
    }

    public virtual void Send<T>(T message, string topic)
    {
        _bus.PubSub.PublishAsync(message, topic);
    }

    public void Listen<T>(Action<T> handler, string topic)
    {
        _bus.PubSub.SubscribeAsync(topic, handler);
    }
}