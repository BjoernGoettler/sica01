using Microsoft.AspNetCore.Mvc;
using Polly;
using TweetService.DTO;
using TweetService.Interfaces;
using TweetService.Models;

namespace TweetService.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private readonly ITweetRepository _tweetRepository;
        private readonly MessageClient _messageClient;

        public TweetController(ITweetRepository tweetRepository, MessageClient messageClient)
        {
            _tweetRepository = tweetRepository;
            _messageClient = messageClient;
        }
        
        // GET: api/Tweets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetTweets()
            => Ok(await _tweetRepository.GetTweets());
        
        // POST: api/Tweets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tweet>> PostTweet(TweetIn tweet)
            => Ok(await _tweetRepository.PostTweet(tweet));
    }