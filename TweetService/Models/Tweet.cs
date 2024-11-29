namespace TweetService.Models;

public class Tweet
{
    public string TweetId { get; set; } = new Guid().ToString();
    public string TweetText { get; set; }   
}