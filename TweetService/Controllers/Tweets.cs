using MessageClient;
using Monitoring;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TweetService.Models;
using Messages;
using MessageClient.Factory;

namespace TweetService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Tweets : ControllerBase
    {
        private static readonly EasyNetQFactory EasyNetQFactory = new EasyNetQFactory();
        //private readonly MessageClient<UserRequest> _messageClient = EasyNetQFactory.CreateTopicMessageClient<UserRequest>("TweetService", "validateUser");
        private readonly MessageClient<UserRequest> _messageClient = EasyNetQFactory.CreateSendReceiveMessageClient<UserRequest>("Users");
        private readonly TweetContext _context;

        public Tweets(TweetContext context)
        {
            _context = context;
            _messageClient.Connect();
        }
        
        // GET: api/Tweets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetTweet()
        {
            
            MonitorService.Log.Here().Debug("GetTweets");
            return await _context.Tweet.ToListAsync();
        }

        // GET: api/Tweets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tweet>> GetTweet(int id)
        {
            MonitorService.Log.Here().Debug("Get tweet with id:" + id.ToString());
            var tweet = await _context.Tweet.FindAsync(id);

            if (tweet == null)
            {
                return NotFound();
            }

            return tweet;
        }
        
        // POST: api/Tweets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tweet>> PostTweet(Tweet tweet)
        {
            var userRequest = new UserRequest
            {
                UserId = tweet.TweetId
            };
            
            MonitorService.Log.Here().Debug("Attempting to validate user through RabbitMQ");
            _messageClient.Send(userRequest);
            
            MonitorService.Log.Here().Debug("PostTweet: " + tweet.TweetId.ToString());
            _context.Tweet.Add(tweet);
            await _context.SaveChangesAsync();
            MonitorService.Log.Here().Information("\ud83d\ude02 we made a tweet : " + tweet.TweetText);
            return CreatedAtAction("GetTweet", new { id = tweet.TweetId }, tweet);
        }

        // DELETE: api/Tweets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTweet(int id)
        {
            MonitorService.Log.Here().Debug("DeleteTweet: " + id.ToString());
            var tweet = await _context.Tweet.FindAsync(id);
            if (tweet == null)
            {
                return NotFound();
            }

            _context.Tweet.Remove(tweet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TweetExists(int id)
        {
            return _context.Tweet.Any(e => e.TweetId == id);
        }

        
    }
}
