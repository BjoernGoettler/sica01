using Microsoft.EntityFrameworkCore;

namespace UserService.Models;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {}
    
    public DbSet<User> User { get; set; }
}