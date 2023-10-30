using LogDemo.Models;

namespace LogDemo.DataTransfer
{
    public static class TracksExtensions
    {
        public static List<TrackDTO> ToDto(this IEnumerable<Track> tracks) =>
            tracks.Select(track => new TrackDTO(track)).ToList();
    }
}
