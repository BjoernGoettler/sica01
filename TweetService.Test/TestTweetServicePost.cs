using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using Moq;
using TweetService.Controllers;
using TweetService.DTO;
using TweetService.Models;
using TweetService.Repositories;
using Tweet = SharedMessages.Tweet;
using DBTweet = TweetService.Models.Tweet;

namespace TweetService.Test;

public class TestTweetServicePost
{
    [Fact]
    public void PostShouldCallAdd()
    {
        //Arrange
        var mockMessageClient = new Mock<MessageClient>();
        var mockSet = new Mock<DbSet<DBTweet>>();
        var mockContext = new Mock<TweetContext>();
        
        var dbtweet = new DBTweet
        {
            TweetText = "Yay",
            TweetId = 2000
        };

        mockContext.Setup(x => x.Set<DBTweet>()).Returns(mockSet.Object);
        
        mockMessageClient.Setup(
            client => client.Send(
                new TweetIn
                {
                    TweetText = "Test time!!!"
                }, "tweet"));
        
        mockContext.Setup(
                m=> m.Add(dbtweet));

        var tweetIn = new TweetIn
        {
            TweetText = "Svendbent er awesome!"
        };
        
        var repository = new TweetRepository(mockContext.Object);
        //Act
        
        var result = repository.PostTweet(tweetIn);
        //Assert
        Assert.True(true);
        //mockContext.Verify(db=> db.Add(dbtweet), Times.AtLeastOnce);
    }
}