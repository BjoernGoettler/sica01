using TweetService.DTO;
using TweetService.Models;

namespace TweetService.Interfaces;

public interface ITweetRepository
{
    Task<IEnumerable<Tweet>> GetTweets();
    Task<Tweet> PostTweet(TweetIn tweet);
}