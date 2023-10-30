using LogDemo.Models;
using LogDemo.Repositories;

namespace LogDemo.Services
{
    public class MusicService : IMusicService
    {
        private readonly IRepository<Track> tracks;
        private readonly IRepository<Genre> genres;

        public MusicService(IRepository<Track> tracks, IRepository<Genre> genres) 
        {
            this.tracks = tracks;
            this.genres = genres;
        }

        public void AddTrack(Track track)
        {
            tracks.Insert(track);
        }

        public void DeleteTrack(int trackId)
        {
            tracks.Delete(trackId);
        }

        public Genre? FindGenreById(int id)
        {
            return genres.Find(id);
        }

        public Track? FindTrackById(int id)
        {
            return tracks.Find(id);
        }

        public List<Track> GetTracks()
        {
            return tracks.GetAll();
        }

        public List<Track> GetByGenre(int genreId)
        {
            return tracks.Where(t => t.GenreId == genreId);
        }

        public List<Genre> GetGenres()
        {
            return genres.GetAll();
        }

        public void UpdateTrack(int id, Track track)
        {
            tracks.Update(id, track);
        }

    }
}
