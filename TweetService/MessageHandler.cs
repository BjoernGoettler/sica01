using EasyNetQ;
using Monitoring;
using TweetService.Models;
using Tweet = SharedMessages.Tweet;

namespace TweetService;

public class MessageHandler: BackgroundService
{
    private readonly TweetContext _context;

    public MessageHandler(TweetContext context)
    {
        _context = context;
    }
    private void HandleTweet(Tweet tweet)
    {
        MonitorService.Log.Information($"Tweet Received: {tweet.TweetText}");
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