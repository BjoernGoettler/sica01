using EasyNetQ;
using SharedMessages;
using Monitoring;

namespace UserService;

public class MessageHandler: BackgroundService
{
    private void HandleTweet(Tweet tweet)
    {
        MonitorService.Log.Information($"Tweet Send: {tweet.TweetText}");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        MonitorService.Log.Information("MessageHandler is running");

        var messageclient = new MessageClient(RabbitHutch.CreateBus("host=rabbitmq;port=5672;virtualHost=/;username=guest;password=guest"));
        
        messageclient.Listen<Tweet>(HandleTweet, "tweet");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}