using LogDemo.Models;
using System.ComponentModel.DataAnnotations;

namespace LogDemo.DataTransfer
{
    public class TrackDTO
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 2)]

        public string Author { get; set; } = string.Empty;

        public string Duration { get; set; }

        public int GenreId { get; set; }

        public TrackDTO() 
        {
        }

        public TrackDTO(Track track)
        {
            Id = track.Id;
            Author = track.Author;
            Title = track.Title;
            Duration = TimeSpan.FromSeconds(track.DurationInSeconds).ToString(@"hh\:mm\:ss");
            GenreId = track.GenreId;
        }

        public Track ToTrack()
        {
            return new Track
            {
                Id = 0,
                Author = Author,
                GenreId = GenreId,
                Title = Title,
                DurationInSeconds = (int)TimeSpan.Parse(Duration).TotalSeconds
            };
        }


    }
}
