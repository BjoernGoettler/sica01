using Microsoft.EntityFrameworkCore;
using Polly;
using TweetService.DTO;
using TweetService.Interfaces;
using TweetService.Models;

namespace TweetService.Repositories;

public class TweetRepository: ITweetRepository
{ 
    private readonly AsyncPolicy _policy;
    private readonly TweetContext _tweetContext;

    public TweetRepository(TweetContext tweetContext)
    {
        _tweetContext = tweetContext;
        _policy = Policy.Handle<Exception>().WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
    public async Task<Tweet> PostTweet(TweetIn tweet)
    {
        var newTweet = new Tweet { TweetText = tweet.TweetText };

        if (await this.TweetExists(newTweet.TweetId))
        {
            return newTweet;
        }

        await _policy.ExecuteAsync(() =>
            AddTweet(newTweet)
        );

        return newTweet;
    }

    
    /// <summary>
    /// Private method to show case the Fallback Policy of the public method
    /// </summary>
    /// <param name="tweet"></param>
    /// <returns>
    /// A Tweet
    /// </returns>
    private async Task<Tweet> AddTweet(Tweet tweet)
    {
        await _tweetContext.Tweet.AddAsync(tweet);
        await _tweetContext.SaveChangesAsync();
        return tweet;
    }

    public async Task<IEnumerable<Tweet>> GetTweets()
        => await _tweetContext.Tweet.ToListAsync();
    
    private async Task<bool> TweetExists(string id)
        => await _tweetContext.Tweet.AnyAsync(e => e.TweetId == id);
}