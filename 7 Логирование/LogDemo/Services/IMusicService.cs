using LogDemo.Models;

namespace LogDemo.Services
{
    public interface IMusicService
    {
        List<Track> GetTracks();
        List<Genre> GetGenres();

        List<Track> GetByGenre(int genreId);

        Track? FindTrackById(int id);
        Genre? FindGenreById(int id);

        void AddTrack(Track track);
        void UpdateTrack(int id, Track track);
        void DeleteTrack(int trackId);
    }
}
