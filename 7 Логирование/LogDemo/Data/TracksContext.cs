using LogDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace LogDemo.Data
{
    public class TracksContext : DbContext
    {
        public TracksContext(DbContextOptions options) : base(options)  { }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}
