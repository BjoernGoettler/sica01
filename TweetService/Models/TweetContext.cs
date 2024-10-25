using Microsoft.EntityFrameworkCore;
namespace TweetService.Models;

public class TweetContext: DbContext
{
    public TweetContext(DbContextOptions<TweetContext> options)
        : base(options)
    {}
    
    public TweetContext()
    {}
    
    public DbSet<Tweet> Tweet { get; set; }
}