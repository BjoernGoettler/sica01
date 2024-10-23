namespace SharedMessages;

public class Tweet
{
    public string TweetText { get; set; }
}

public class TweetReply
{
    public int TweetId { get; set; }
    public string TweetText { get; set; }
}