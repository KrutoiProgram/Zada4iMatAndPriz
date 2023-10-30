using System.ComponentModel.DataAnnotations;

namespace LogDemo.Models
{
    public class Track
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(100)]
        public string Author { get; set; } = string.Empty;

        public int DurationInSeconds { get; set; }

        public int GenreId { get; set; }
        
        public Genre Genre { get; set; } = null!;

    }
}
