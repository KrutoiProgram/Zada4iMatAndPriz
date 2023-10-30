using LogDemo.Data;
using LogDemo.DataTransfer;
using LogDemo.Errors;
using LogDemo.Models;
using LogDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService music;

        public MusicController(IMusicService music) 
        {
            this.music = music;
        }


        [HttpGet("tracks")]
        public List<TrackDTO> GetTracks()
        {
            return music.GetTracks().ToDto();
        }

        [HttpGet("track/{id}")]
        public IActionResult GetTrack(int id)
        {
            var track = music.FindTrackById(id);
            if (track is null)
                return NotFound();
            return Ok(new TrackDTO(track));
        }

        [HttpGet("tracks/{genreId}")]
        public List<TrackDTO> GetByGenre(int genreId)
        {
            return music.GetByGenre(genreId).ToDto();
        }

        [HttpGet("genres")]
        public List<Genre> GetGenres()
        {
            return music.GetGenres();
        }

        [HttpPost("track")]
        public IActionResult AddTrack(TrackDTO trackDto)
        {
            music.AddTrack(trackDto.ToTrack());
            return Ok();
        }


        [HttpPut("track")]
        public IActionResult UpdateTrack(int trackId, TrackDTO trackDto)
        {
            music.UpdateTrack(trackId, trackDto.ToTrack());
            return Ok();
        }

        [HttpDelete("track")]
        public IActionResult DeleteTrack(int trackId)
        {
            try
            {
                music.DeleteTrack(trackId);
                return Ok();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }


    }
}
