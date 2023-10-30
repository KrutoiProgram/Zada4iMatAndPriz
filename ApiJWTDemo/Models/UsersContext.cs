using Microsoft.EntityFrameworkCore;

namespace ApiJwtDemo.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions options) : base(options)
        { 
        }

        public virtual DbSet<User> Users { get; set; }
    }
}
