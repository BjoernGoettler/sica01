using Microsoft.EntityFrameworkCore;
using TweetService.DTO;
using TweetService.Interfaces;
using TweetService.Models;

namespace TweetService.Repositories;

public class TweetRepository(TweetContext tweetContext): ITweetRepository
{ 
    public async Task<Tweet> PostTweet(TweetIn tweet)
    {
        var newTweet = new Tweet { TweetText = tweet.TweetText };
        tweetContext.Tweet.Add(newTweet);
        await tweetContext.SaveChangesAsync();
        return newTweet;
    }

    public async Task<IEnumerable<Tweet>> GetTweets()
        => await tweetContext.Tweet.ToListAsync();
    
    internal async Task<bool> TweetExists(int id)
    {
        return await tweetContext.Tweet.AnyAsync(e => e.TweetId == id);
    }
}