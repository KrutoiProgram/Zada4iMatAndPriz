using System.ComponentModel.DataAnnotations;

namespace LogDemo.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

    }
}